 using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management.Instrumentation;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Cuda;
using Emgu.CV.CvEnum;
using Emgu.CV.Flann;
using Emgu.CV.Reg;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace MatchingTool
{
    public partial class Form1 : Form
    {
        Image<Bgr, byte> imgInput;
        Image<Bgr, byte> imgInputTrain;
        Image<Bgr, byte> imgTemplate;
        Point StartROI, EndROI;
        bool selecting, MouseDown;
        bool InpaintMouseDown = false;
        bool InpaintSelection = false;
        List<List<Point>> InpaintPoints = null;
        List<Point> InpaintCurrentPoints = null;
        Rectangle rectROI;
        Rectangle rect;
        double thresValue;
        Rectangle rectTemplate;
        List<Point> pointRectROI;
        List<Image<Gray, byte>> listImageTemplate;
        VectorOfVectorOfPoint contourTemplate = new VectorOfVectorOfPoint();
        double angle = 0;
        int indexContourTem = -1;
        Rectangle recROI2 ;
        List<Point> listPointRectROI2;
        int numMaxSearch;
        public Form1()
        {
            InitializeComponent();
            txbPyramid.Text = trackLevelPyramid.Value.ToString();
        }

        

        //Lấy ảnh
        private void btnFile_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog of = new OpenFileDialog();
                if (of.ShowDialog() == DialogResult.OK)
                {
                    string path = of.FileName;
                    imgInput = new Image<Bgr, byte>(path);
                    picImage.Image = imgInput.ToBitmap();
                                                    
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        

        private void picImage_MouseDown(object sender, MouseEventArgs e)
        {
            if (InpaintSelection == true && e.Button == MouseButtons.Left)
            {
                InpaintMouseDown = true;
                InpaintCurrentPoints.Add(e.Location);
            }

            if (selecting)
            {
                MouseDown = true;
                StartROI = e.Location;
            }
        }


        private void picImage_MouseMove(object sender, MouseEventArgs e)
        {
            if (picImage.Image == null)
            {
                return;
            }
            if (InpaintMouseDown == true && InpaintSelection == true)
            {
                if (InpaintCurrentPoints.Count > 0)
                {
                    Pen p = new Pen(Brushes.Red, 5);
                    using (Graphics g = Graphics.FromImage(picImage.Image))
                    {
                        g.DrawLine(p, InpaintCurrentPoints.Last(), e.Location);
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                    }
                }
                InpaintCurrentPoints.Add(e.Location);
                picImage.Invalidate();
            }
            if (selecting)
            {
                int width = Math.Max(StartROI.X, e.X) - Math.Min(StartROI.X, e.X);
                int height = Math.Max(StartROI.Y, e.Y) - Math.Min(StartROI.Y, e.Y);
                rect = new Rectangle(Math.Min(StartROI.X, e.X),
                    Math.Min(StartROI.Y, e.Y),
                    width,
                    height);
                ///Tính zoom
                float zoomx = 0;
                float zoomy = 0;
                zoomx = (float)picImage.Width / (float)imgInput.Width;
                zoomy = (float)picImage.Height / (float)imgInput.Height;
                int imgx = 0;
                int imgy = 0;
                imgx = (int)(rect.X / zoomx);
                imgy = (int)(rect.Y / zoomy);
                int w = (int)(width / zoomx);
                int h = (int)(height / zoomy);
                rectROI = new Rectangle(imgx, imgy, w, h);

                Refresh();
            }
        }

        
        private void picImage_Paint(object sender, PaintEventArgs e)
        {
            if (MouseDown)
            {
                using (Pen pen = new Pen(Color.Red, 3))
                {
                    e.Graphics.DrawRectangle(pen, rect);
                }
            }
        }


        private void picImage_MouseUp(object sender, MouseEventArgs e)
        {
            if (InpaintMouseDown == true && InpaintSelection)
            {
                InpaintMouseDown = false;
                InpaintPoints.Add(InpaintCurrentPoints.ToList());
                InpaintCurrentPoints.Clear();
            }

            if (selecting)
            {
                selecting = false;
                MouseDown = false;
            }
        }

        private void txbPyramid_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Xác thực rằng phím vừa nhấn không phải CTRL hoặc không phải dạng số
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void txbMinScore_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Xác thực rằng phím vừa nhấn không phải CTRL hoặc không phải dạng số
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }

        private void txbNumSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Xác thực rằng phím vừa nhấn không phải CTRL hoặc không phải dạng số
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }
        }
        private void trackLevelPyramid_Scroll(object sender, EventArgs e)
        {
            txbPyramid.Text = trackLevelPyramid.Value.ToString();
        }

        //Chọn vẽ ROI
        private void btnDrawROI_Click(object sender, EventArgs e)
        {
            selecting = true;
        }

        

        //Lấy ROI template
        private void btnGetROI1_Click(object sender, EventArgs e)
        {
            try
            {
                if (picImage.Image == null)
                    return;

                if (rect == Rectangle.Empty)
                    return;
                imgTemplate = null;
                imgInputTrain = null;
                recROI2 = new Rectangle();
                imgInputTrain = imgInput;
                var img = imgInput;


                img.ROI = rectROI;
                rectTemplate = rectROI;
                var imgROI = img.Copy();
                imgTemplate = imgROI.Clone();
                img.ROI = Rectangle.Empty;
                picTemplate.Image = imgROI.ToBitmap();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnGetROI2_Click(object sender, EventArgs e)
        {
            try
            {
                if (picImage.Image == null)
                    return;

                if (rect == Rectangle.Empty)
                    return;

               recROI2= rectROI;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void chkFindAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkFindAll.Checked)
                txbNumSearch.Enabled = false;
            else
                txbNumSearch.Enabled = true;
        }
        private void btnTrain_Click(object sender, EventArgs e)
        {
            if (imgInput == null)
                return;
            if(imgTemplate == null)
                return;

            try
            {
                
                int levelPyramid = trackLevelPyramid.Value;
                bool isInv = chkInv.Checked;
                //TrainTemplate(imgTemplate, levelPyramid, isInv, rectTemplate, recROI2, out listImageTemplate, out thresValue, out angle,
                //    out Point origin, out indexContourTem, out contourTemplate, out Image<Bgr, byte> imgOut, out pointRectROI, out listPointRectROI2);
                //picTemplate.Image = imgOut.AsBitmap();
                TrainTemplate(imgInputTrain, imgTemplate, levelPyramid, isInv, rectTemplate, recROI2, out listImageTemplate, out thresValue, out angle,
                    out Point origin, out indexContourTem, out contourTemplate, out Image<Bgr, byte> imgOut, out pointRectROI, out listPointRectROI2);
                picTemplate.Image = imgOut.ToBitmap();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnMatching_Click(object sender, EventArgs e)
        {
            if (listImageTemplate==null)
            {
                MessageBox.Show("Chưa train");
                return;
            }
            int levelPyramid = trackLevelPyramid.Value;
            numMaxSearch = Convert.ToInt32(txbNumSearch.Text);
            if (chkFindAll.Checked)
                numMaxSearch = 100;
            double minScore = Convert.ToDouble(txbMinScore.Text);
            bool isInv = chkInv.Checked;
            var imgInputMatching = imgInput.Clone();
            Matching(imgInputMatching, levelPyramid, isInv, thresValue, listImageTemplate,
                contourTemplate, indexContourTem, angle, numMaxSearch, minScore, pointRectROI, listPointRectROI2,
                out Image<Bgr, byte> imgOut, out string matchingTime, out int numberFind,
                out List<ValueTuple<Point, double>> listOrigin);
            lbNumFind.Text = numberFind.ToString();
            lbTime.Text = matchingTime + " ms";
            picImage.Image = imgOut.AsBitmap();
        }


        /// <summary>
        /// Nhiệm vụ của function này là train Template
        /// </summary>
        /// <param name="imgInput">Ảnh chứa ảnh Template</param>
        /// <param name="imgTemplate">Ảnh Template</param>
        /// <param name="levelPyramid">Mức Pyramid. Có các mức từ 0 đến 8</param>
        /// <param name="isInv">isIns = true khi background sáng,isIns = true khi background tối</param>
        /// <param name="rectTemplate">Rectangle của ROI template</param>
        /// <param name="recROI2">Rectangle của ROI thứ 2 </param>
        /// <param name="listImgTemplate">list ảnh Template sau khi train (đầu vào của funtion Matching)</param>
        /// <param name="thresValue">Giá trị Threshold (đầu vào của funtion Matching) </param>
        /// <param name="angle">Góc của đối tượng template</param>
        /// <param name="originPoint">Điểm origin của đối tượng</param>
        /// <param name="indexCntTem">chỉ số của contour đối tượng Template</param>
        /// <param name="templateContour">Các contour của ảnh template</param>
        /// <param name="imgOut">Ảnh đầu ra</param>
        /// <param name="lpointRectROI">List các điểm của rectTemplate sau khi chuyển sang origin của đối tượng template</param>
        /// <param name="listPointRectROI2">List các điểm của ROI 2 sau khi chuyển sang origin của đối tượng template</param>

        private void TrainTemplate(
            Image<Bgr, byte> imgInput,
            Image<Bgr, byte> imgTemplate,
            int levelPyramid,
            bool isInv,
            Rectangle rectTemplate,
            Rectangle recROI2,
            out List<Image<Gray, byte>> listImgTemplate,
            out double thresValue,
            out double angle,
            out Point originPoint,
            out int indexCntTem,
            out VectorOfVectorOfPoint templateContour,
            out Image<Bgr, byte> imgOut,
            out List<Point> lpointRectROI,
            out List<Point> listPointRectROI2
            )
        {
            lpointRectROI = new List<Point>();
            listPointRectROI2 = new List<Point>();
            listImgTemplate = new List<Image<Gray, byte>>();
            imgOut = imgTemplate.Convert<Bgr, byte>();

            //Tính toán tọa độ tâm HCN của ROI ban đầu
            var xCenterROI = rectTemplate.Location.X + rectTemplate.Width / 2;
            var yCenterROI = rectTemplate.Location.Y + rectTemplate.Height / 2;

            //Tính toán HCN ROI mới ứng với mỗi góc xoay, crop lại ảnh,xoay ảnh
            for (int i = 0; i < 360; i += 5)
            {
                ///Tính height và width của HCN mới
                var alpha = i * Math.PI / 180;
                double w = (double)(Math.Abs(rectTemplate.Height * Math.Sin(alpha)) + Math.Abs(rectTemplate.Width * Math.Cos(alpha)));
                double h = (double)(Math.Abs(rectTemplate.Height * Math.Cos(alpha)) + Math.Abs(rectTemplate.Width * Math.Sin(alpha)));

                ///Tính Location của HCN mới
                int xRect = (int)(xCenterROI - w / 2);
                int yRect = (int)(yCenterROI - h / 2);

                //Xoay Input ảnh
                var imgRotate = imgInput.Rotate(i, new PointF((float)xCenterROI, (float)yCenterROI),Inter.Cubic ,new Bgr(0,0,0),true);
                Rectangle r = new Rectangle(new Point(xRect, yRect), new Size((int)w, (int)h));
                var img = imgRotate.Convert<Gray, byte>();
                img.ROI = r;
                var imgROI = img.Copy();
                img.ROI = Rectangle.Empty;
                //Pyramid ảnh
                Image<Gray, byte> imgPyramid;
                if (levelPyramid == 8)
                    imgPyramid = imgROI.Resize(0.03125, Inter.NearestExact);
                else if (levelPyramid == 7)
                    imgPyramid = imgROI.Resize(0.04, Inter.NearestExact);
                else if (levelPyramid == 6)
                    imgPyramid = imgROI.Resize(0.0625, Inter.NearestExact);
                else if (levelPyramid == 5)
                    imgPyramid = imgROI.Resize(0.085, Inter.NearestExact);
                else if (levelPyramid == 4)
                    imgPyramid = imgROI.Resize(0.125, Inter.NearestExact);
                else if (levelPyramid == 3)
                    imgPyramid = imgROI.Resize(0.18, Inter.NearestExact);
                else if (levelPyramid == 2)
                    imgPyramid = imgROI.Resize(0.25, Inter.NearestExact);
                else if (levelPyramid == 1)
                    imgPyramid = imgROI.Resize(0.5, Inter.NearestExact);
                else
                    imgPyramid = imgROI.Clone();
               
                listImgTemplate.Add(imgPyramid);
            }


            Image<Gray, byte> imgTem = imgTemplate.Convert<Gray, byte>();

            //Tìm hướng của template
            Mat hierarchy = new Mat();
            Image<Gray, byte> imgTemThres = new Image<Gray, byte>(imgTem.Width, imgTem.Height);

            //////Threshold
            thresValue = CvInvoke.Threshold(imgTem, imgTemThres, 125, 255, ThresholdType.Otsu);
            if (isInv)
            {
                CvInvoke.Threshold(imgTemThres, imgTemThres, 125, 255, ThresholdType.BinaryInv);
            }

            VectorOfVectorOfPoint contoursTem = new VectorOfVectorOfPoint();
            CvInvoke.FindContours(imgTemThres, contoursTem, hierarchy, RetrType.Ccomp, ChainApproxMethod.ChainApproxSimple);
            Array arrHier = hierarchy.GetData();
            //Tìm contour có diện tích lớn nhất
            double areaContourMax = 0;
            int indexContourMax = -1;
            for (int i = 0; i < contoursTem.Size; i++)
            {
                if (CvInvoke.ContourArea(contoursTem[i]) > areaContourMax)
                {
                    areaContourMax = CvInvoke.ContourArea(contoursTem[i]);
                    indexContourMax = i;
                }
            }
            indexCntTem = indexContourMax;
            templateContour = contoursTem;

            //Tìm tâm contour
            Moments moments = CvInvoke.Moments(templateContour[indexCntTem]);
            Point center = new Point();
            center.X = (int)(moments.M10 / moments.M00);
            center.Y = (int)(moments.M01 / moments.M00);

            // Tìm HCN bao quanh contour
            RotatedRect rect = CvInvoke.MinAreaRect(templateContour[indexCntTem]);
            /// Lấy tọa độ các đỉnh Hcn
            var v = rect.GetVertices();
            ///Tìm trung điểm các cạnh HCN
            Point[] midpointEdge = new Point[4];
            midpointEdge[0].X = (int)(v[0].X + v[1].X) / 2;
            midpointEdge[0].Y = (int)(v[0].Y + v[1].Y) / 2;
            midpointEdge[1].X = (int)(v[1].X + v[2].X) / 2;
            midpointEdge[1].Y = (int)(v[1].Y + v[2].Y) / 2;
            midpointEdge[2].X = (int)(v[2].X + v[3].X) / 2;
            midpointEdge[2].Y = (int)(v[2].Y + v[3].Y) / 2;
            midpointEdge[3].X = (int)(v[3].X + v[0].X) / 2;
            midpointEdge[3].Y = (int)(v[3].Y + v[0].Y) / 2;

            //Tìm tâm của các contour con
            Point pointCenterChild = new Point();
            int numberChild = 0;
            int indexChild = (int)arrHier.GetValue(0, indexContourMax, 2);
            while (indexChild != -1)
            {
                if (CvInvoke.ContourArea(contoursTem[indexChild]) > areaContourMax * 0.1)
                {
                    Moments momentChild = CvInvoke.Moments(contoursTem[indexChild]);
                    Point centerChild = new Point();
                    centerChild.X = (int)(momentChild.M10 / momentChild.M00);
                    centerChild.Y = (int)(momentChild.M01 / momentChild.M00);
                    pointCenterChild.X += centerChild.X;
                    pointCenterChild.Y += centerChild.Y;
                    numberChild++;
                }
                indexChild = (int)arrHier.GetValue(0, indexChild, 0);
            }

            //Tìm trọng tâm khi có các contour con (nếu có)
            Point centerPoint = center;
            if (numberChild != 0)
            {
                pointCenterChild.X = pointCenterChild.X / numberChild;
                pointCenterChild.Y = pointCenterChild.Y / numberChild;

                centerPoint.X = (int)(pointCenterChild.X + center.X) / 2;
                centerPoint.Y = (int)(pointCenterChild.Y + center.Y) / 2;
            }
            //Tìm trung điểm HCN có khoảng cách lớn nhất đến điểm trọng tâm
            double distanceMax = 0;
            int indexPTDMax = -1;
            for (int i = 0; i < 4; i++)
            {
                double distance = Math.Sqrt(Math.Pow(midpointEdge[i].X - centerPoint.X, 2) + Math.Pow(midpointEdge[i].Y - centerPoint.Y, 2));
                if (distance > distanceMax)
                {
                    distanceMax = distance;
                    indexPTDMax = i;
                }
            }
            //Tính góc của Hướng so với trục Ox
            /// Tìm Vecto của Hướng
            Point direcVec = new Point();
            direcVec.X = midpointEdge[indexPTDMax].X - (int) rect.Center.X;
            direcVec.Y = midpointEdge[indexPTDMax].Y - (int) rect.Center.Y;

            ///Vecto chỉ hướng của trục Ox là (1;0)
            ///Tính cos của góc tạo bởi vecto hướng với Ox
            double CosAngle = (double)direcVec.X / Math.Sqrt(Math.Pow(direcVec.X, 2) + Math.Pow(direcVec.Y, 2));
            ///Tính góc
            angle = Math.Acos(CosAngle) * 180 / Math.PI;
            ///Fix về 360 độ
            if (midpointEdge[indexPTDMax].Y < rect.Center.Y)
            {
                angle = -angle;
            }

            originPoint = center;
            ///Xử lí recROI1
            Point r1 = new Point();
            Point r2 = new Point();
            Point r3 = new Point();
            Point r4 = new Point();
            r1.X = rectTemplate.X;
            r1.Y = rectTemplate.Y;
            r2.X = rectTemplate.X + rectTemplate.Width;
            r2.Y = rectTemplate.Y;
            r3.X = rectTemplate.X + rectTemplate.Width;
            r3.Y = rectTemplate.Y + rectTemplate.Height;
            r4.X = rectTemplate.X;
            r4.Y = rectTemplate.Y + rectTemplate.Height;

            Point n1, n2, n3, n4;

            n1 = ConvertCoordinatesToNewOrigin(r1, center, angle, r1);
            n2 = ConvertCoordinatesToNewOrigin(r1, center, angle, r2);
            n3 = ConvertCoordinatesToNewOrigin(r1, center, angle, r3);
            n4 = ConvertCoordinatesToNewOrigin(r1, center, angle, r4);

            lpointRectROI.Add(n1);
            lpointRectROI.Add(n2);
            lpointRectROI.Add(n3);
            lpointRectROI.Add(n4);

            ///Xử lí recROI2
            if (recROI2.Width != 0)
            {
                Point r11 = new Point();
                Point r12 = new Point();
                Point r13 = new Point();
                Point r14 = new Point();
                r11.X = recROI2.X;
                r11.Y = recROI2.Y;
                r12.X = recROI2.X + recROI2.Width;
                r12.Y = recROI2.Y;
                r13.X = recROI2.X + recROI2.Width;
                r13.Y = recROI2.Y + recROI2.Height;
                r14.X = recROI2.X;
                r14.Y = recROI2.Y + recROI2.Height;

                Point n11, n12, n13, n14;

                n11 = ConvertCoordinatesToNewOrigin(r1, center, angle, r11);
                n12 = ConvertCoordinatesToNewOrigin(r1, center, angle, r12);
                n13 = ConvertCoordinatesToNewOrigin(r1, center, angle, r13);
                n14 = ConvertCoordinatesToNewOrigin(r1, center, angle, r14);

                listPointRectROI2.Add(n11);
                listPointRectROI2.Add(n12);
                listPointRectROI2.Add(n13);
                listPointRectROI2.Add(n14);
            }

            CvInvoke.DrawContours(imgOut, contoursTem, indexContourMax, new MCvScalar(255, 0, 0), 2);
            CvInvoke.ArrowedLine(imgOut, center, midpointEdge[indexPTDMax], new MCvScalar(0, 255, 0), 3, LineType.AntiAlias, 0, 0.1);
            CvInvoke.Circle(imgOut, center, 4, new MCvScalar(0, 0, 255), -1);
        }

        /// <summary>
        /// Function có chức năng tìm các vùng giống template
        /// </summary>
        /// <param name="imgInput">Ảnh đầu vào</param>
        /// <param name="listROIFind">List các rectangle tìm được</param>
        /// <param name="listIndexFind">List chỉ số góc quay</param>
        /// <param name="levelPyramid">Mức Pyramid. Có các mức từ 0 đến 8</param>
        /// <param name="listImgTemplate">list ảnh Template. Đầu ra của function TrainTemplate  </param>
        /// <param name="indexStart">Chỉ số tìm kiếm đầu tiên</param>
        /// <param name="indexEnd">Chỉ số tìm kiếm cuối</param>
        /// <param name="numMaxSearch">Số lượng tìm kiếm tối đa</param>
        /// <param name="minScore">Score nhỏ nhất</param>
        private void FindTemplate(
            Image<Gray, byte> imgInput,
            List<Rectangle> listROIFind,
            List<int>listIndexFind,
            int levelPyramid,
            List<Image<Gray, byte>> listImgTemplate,
            int indexStart,
            int indexEnd,
            int numMaxSearch,
            double minScore
            )
        {
            bool whileEnded = false;
            while(!whileEnded && listROIFind.Count< numMaxSearch)
            {
                Rectangle r = Rectangle.Empty;
                double globalMinVal = 0;

                int indexFind = -1;
                int index = -1;
                for (int k = indexStart; k <= indexEnd; k++)
                {
                    index = k;
                    if (index < 0)
                    {
                        index = 72 + index;
                    }
                    Mat imgout = new Mat();
                    CvInvoke.MatchTemplate(imgInput, listImgTemplate[index], imgout, TemplateMatchingType.CcoeffNormed);

                    double minval = 0;
                    double maxval = 0;
                    Point minloc = new Point();
                    Point maxloc = new Point();

                    CvInvoke.MinMaxLoc(imgout, ref minval, ref maxval, ref minloc, ref maxloc);



                    if (maxval > globalMinVal && maxval > minScore)
                    {
                        globalMinVal = maxval;
                        maxloc.X = (int)(maxloc.X - listImageTemplate[index].Width * 0.05);
                        maxloc.Y =(int)( maxloc.Y - listImageTemplate[index].Height * 0.05);
                        Size sr = new Size((int)(listImgTemplate[index].Width * 1.2),(int)( listImgTemplate[index].Height * 1.2));
                        r = new Rectangle(maxloc, sr);
                        
                        indexFind = index;
                    }
                }
                if (r != Rectangle.Empty)
                {
                    if (listImageTemplate[0].Width > listImageTemplate[0].Height)
                        CvInvoke.Circle(imgInput,
                            new Point(r.X + (int)(listImageTemplate[index].Width * 0.55), r.Y + (int)(listImageTemplate[index].Height * 0.55)),
                            (int)(listImageTemplate[0].Height / 2),new MCvScalar(0), -1);
                    else
                        CvInvoke.Circle(imgInput,
                           new Point(r.X + (int)(listImageTemplate[index].Width * 0.55), r.Y + (int)(listImageTemplate[index].Height * 0.55)),
                           (int)(listImageTemplate[0].Width / 2), new MCvScalar(0), -1);
                    //CvInvoke.Rectangle(imgInput, r, new MCvScalar(255, 255, 255), -1);
                    double level = 0;
                    if(levelPyramid==8)
                    {
                        level = (double)(1 / 0.03125);
                    }
                    else if(levelPyramid == 7)
                    {
                        level = (double)(1 / 0.04);
                    }
                    else if (levelPyramid == 6)
                    {
                        level = (double)(1 / 0.0625);
                    }
                    else if (levelPyramid == 5)
                    {
                        level = (double)(1 / 0.085);
                    }
                    else if (levelPyramid == 4)
                    {
                        level = (double)(1 / 0.125);
                    }
                    else if (levelPyramid == 3)
                    {
                        level = (double)(1 / 0.18);
                    }
                    else if (levelPyramid == 2)
                    {
                        level = (double)(1 / 0.25);
                    }
                    else if (levelPyramid == 1)
                    {
                        level = (double)(1 / 0.5);
                    }
                    else
                    {
                        level = 0;
                    }
                    Rectangle rectangle = new Rectangle((int)(r.X * level), (int)(r.Y * level)
                    , (int)(r.Width * level), (int)(r.Height * level));
                    listROIFind.Add(rectangle);
                    listIndexFind.Add(indexFind);
                }
                else
                {
                    whileEnded = true;
                }
            }
        }


        /// <summary>
        /// Chức năng của hàm này là tìm kiếm đối tượng, origin của đối tượng giống template
        /// </summary>
        /// <param name="ImgInput">Ảnh đầu vào</param>
        /// <param name="levelPyramid">Mức Pyramid. Có các mức từ 0 đến 8 </param>
        /// <param name="isInv">isIns = true khi background sáng,isIns = true khi background tối </param>
        /// <param name="thresValue">Giá trị Threshold (đầu ra của funtion TrainTemplate) </param>
        /// <param name="listImgTemplate">list ảnh Template sau khi train (đầu ra của funtion TrainTemplate)</param>
        /// <param name="templateContour">Các contour của ảnh template (đầu ra của funtion TrainTemplate)</param>
        /// <param name="indexCntTem">chỉ số của contour đối tượng Template (đầu ra của funtion TrainTemplate)</param>
        /// <param name="angleTemplate">Góc của đối tượng template (đầu ra của funtion TrainTemplate)</param>
        /// <param name="numMaxSearch">Số lượng tìm kiếm tối đa </param>
        /// <param name="minScore">Score nhỏ nhất </param>
        /// <param name="pointRectROI">List các điểm của rectTemplate sau khi chuyển sang origin của đối tượng template (đầu ra của funtion TrainTemplate)</param>
        /// <param name="listPointRectROI2">List các điểm của ROI 2 sau khi chuyển sang origin của đối tượng template (đầu ra của funtion TrainTemplate)</param>
        /// <param name="imgOut">Ảnh đầu ra</param>
        /// <param name="matchingTime">Thời gian matching</param>
        /// <param name="numberFind">Số lượng tìm kiếm được</param>
        /// <param name="listOrigin">list Origin của đối tượng tìm kiếm được</param>
        private void Matching(
            Image<Bgr, byte> ImgInput,
            int levelPyramid,
            bool isInv,
            double thresValue,
            List<Image<Gray, byte>> listImgTemplate,
            VectorOfVectorOfPoint templateContour,
            int indexCntTem,
            double angleTemplate,
            int numMaxSearch,
            double minScore,
            List<Point> pointRectROI,
            List<Point> listPointRectROI2,
            out Image<Bgr, byte> imgOut,
            out string matchingTime,
            out int numberFind,
            out List<ValueTuple<Point,double>> listOrigin
            )
        {
            var watch = new System.Diagnostics.Stopwatch();
            watch.Start();
            imgOut = ImgInput.Clone();
            listOrigin = new List<(Point, double)>();
            Image<Gray, byte> imgPyramid;
            if (levelPyramid == 8)
                imgPyramid = ImgInput.Convert<Gray, byte>().Resize(0.03125,Inter.NearestExact);
            else if (levelPyramid == 7)
                imgPyramid = ImgInput.Convert<Gray, byte>().Resize(0.04, Inter.NearestExact);
            else if (levelPyramid == 6)
                imgPyramid = ImgInput.Convert<Gray, byte>().Resize(0.0625, Inter.NearestExact);
            else if (levelPyramid == 5)
                imgPyramid = ImgInput.Convert<Gray, byte>().Resize(0.085, Inter.NearestExact);
            else if (levelPyramid == 4)
                imgPyramid = ImgInput.Convert<Gray, byte>().Resize(0.125, Inter.NearestExact);
            else if (levelPyramid == 3)
                imgPyramid = ImgInput.Convert<Gray, byte>().Resize(0.18, Inter.NearestExact);
            else if (levelPyramid == 2)
                imgPyramid = ImgInput.Convert<Gray, byte>().Resize(0.25, Inter.NearestExact);
            else if (levelPyramid == 1)
                imgPyramid = ImgInput.Convert<Gray, byte>().Resize(0.5, Inter.NearestExact);
            else
                imgPyramid = ImgInput.Convert<Gray, byte>();

            List<Rectangle> listROIFind = new List<Rectangle>();
            List<int> listIndexFind = new List<int>();
            FindTemplate(imgPyramid, listROIFind, listIndexFind, levelPyramid, listImgTemplate,
                -3, 3, numMaxSearch, minScore);
            FindTemplate(imgPyramid, listROIFind, listIndexFind, levelPyramid, listImgTemplate,
                4, 10, numMaxSearch, minScore);
            FindTemplate(imgPyramid, listROIFind, listIndexFind, levelPyramid, listImgTemplate,
                11, 17, numMaxSearch, minScore);
            FindTemplate(imgPyramid, listROIFind, listIndexFind, levelPyramid, listImgTemplate,
                18, 24, numMaxSearch, minScore);
            FindTemplate(imgPyramid, listROIFind, listIndexFind, levelPyramid, listImgTemplate,
                25, 31, numMaxSearch, minScore);
            FindTemplate(imgPyramid, listROIFind, listIndexFind, levelPyramid, listImgTemplate,
                32, 38, numMaxSearch, minScore);
            FindTemplate(imgPyramid, listROIFind, listIndexFind, levelPyramid, listImgTemplate,
                39, 45, numMaxSearch, minScore);
            FindTemplate(imgPyramid, listROIFind, listIndexFind, levelPyramid, listImgTemplate,
                46, 52, numMaxSearch, minScore);
            FindTemplate(imgPyramid, listROIFind, listIndexFind, levelPyramid, listImgTemplate,
                53, 60, numMaxSearch, minScore);
            FindTemplate(imgPyramid, listROIFind, listIndexFind, levelPyramid, listImgTemplate,
                61, 68, numMaxSearch, minScore);
            //CvInvoke.Imshow(" ", imgPyramid);
            //CvInvoke.WaitKey();
            //CvInvoke.DestroyAllWindows();

            //Crop từng vùng tìm được
            List<Image<Bgr, byte>> listImgROI = new List<Image<Bgr, byte>>();
            if (listROIFind != null)
                for (int i = 0; i < listROIFind.Count; i++)
                {
                    var img = imgInput.Convert<Bgr, byte>();
                    img.ROI = listROIFind[i];
                    var imgROI = img.Copy();
                    listImgROI.Add(imgROI);
                    //CvInvoke.Rectangle(imgOut, listROIFind[i], new MCvScalar(0, 0, 255), (int)imgOut.Width / 170);
                }
            
            //Tìm hướng của đối tượng

            if (listImgROI != null)
            {
                int numROI2 = 0;
                for (int i = 0; i < listImgROI.Count; i++)
                {
                    ValueTuple<Point, double> Origin = (new Point(), 0);
                    //Tìm các contour
                    Image<Gray, byte> imgROI = listImgROI[i].Convert<Gray, byte>();
                    Image<Gray, byte> imgRoiThres = new Image<Gray, byte>(imgROI.Width,imgROI.Height);
                    VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint();
                    
                    Mat hierarchy = new Mat();
                    //////Threshold
                    CvInvoke.Threshold(imgROI, imgRoiThres, thresValue, 255, ThresholdType.Binary);
                    if (isInv)
                    {
                        CvInvoke.Threshold(imgRoiThres, imgRoiThres, 125, 255, ThresholdType.BinaryInv);
                    }
                    CvInvoke.FindContours(imgRoiThres, contours, hierarchy, RetrType.Ccomp, ChainApproxMethod.ChainApproxSimple);
                    Array arrHier = hierarchy.GetData();
                    double minDiff = (double)1 / 0;
                    int indexCnt = -1;
                    var areaCntTem = CvInvoke.ContourArea(templateContour[indexCntTem]);
                    // Tìm contour giống contour template
                    for (int j = 0; j < contours.Size; j++)
                    {
                        var distance = CvInvoke.MatchShapes(contours[j], templateContour[indexCntTem], ContoursMatchType.I1);
                        if (distance < minDiff && CvInvoke.ContourArea(contours[j]) > areaCntTem * 0.8&&
                            CvInvoke.ContourArea(contours[j]) < areaCntTem * 1.2)
                        {
                            minDiff = distance;
                            indexCnt = j;
                        }
                    }

                    // Lấy tâm contour
                    Moments moments = CvInvoke.Moments(contours[indexCnt]);
                    Point center = new Point();
                    center.X = (int)(moments.M10 / moments.M00);
                    center.Y = (int)(moments.M01 / moments.M00);
                    
                    //Tính giới hạn góc
                    double angleMin = angleTemplate + listIndexFind[i] * 5 - 15;
                    if (angleMin < -180)
                        angleMin = 360 + angleMin;
                    if (angleMin > 180)
                        angleMin = angleMin - 360;
                    double angleMax = angleTemplate + listIndexFind[i] * 5 + 15;
                    if (angleMax > 180)
                        angleMax = angleMax - 360;

                    // Tìm HCN bao quanh contour
                        RotatedRect rect = CvInvoke.MinAreaRect(contours[indexCnt]);
                    // Lấy tọa độ các đỉnh Hcn
                    var v = rect.GetVertices();
                    
                    ///Tìm trung điểm các cạnh HCN
                    Point[] MPERect = new Point[4];
                    MPERect[0].X = (int)(v[0].X + v[1].X) / 2;
                    MPERect[0].Y = (int)(v[0].Y + v[1].Y) / 2;
                    MPERect[1].X = (int)(v[1].X + v[2].X) / 2;
                    MPERect[1].Y = (int)(v[1].Y + v[2].Y) / 2;
                    MPERect[2].X = (int)(v[2].X + v[3].X) / 2;
                    MPERect[2].Y = (int)(v[2].Y + v[3].Y) / 2;
                    MPERect[3].X = (int)(v[3].X + v[0].X) / 2;
                    MPERect[3].Y = (int)(v[3].Y + v[0].Y) / 2;

                    Point endPoint = new Point();
                    bool loodEnd = false;
                    int k = 0;
                    double angleFind = 0;
                    
                    while (!loodEnd)
                    {
                        Point directVec = new Point();
                        directVec.X = MPERect[k].X - (int)rect.Center.X;
                        directVec.Y = MPERect[k].Y - (int)rect.Center.Y;
                        ///Vecto chỉ hướng của trục Ox là (1;0)
                        ///Tính cos của góc tạo bởi vecto hướng với Ox
                        double CosAlpha = (double)directVec.X / Math.Sqrt(Math.Pow(directVec.X, 2) + Math.Pow(directVec.Y, 2));
                        ///Tính góc
                        angleFind = Math.Acos(CosAlpha) * 180 / Math.PI;
                        bool fDirect = false;
                        ///Fix về 360 độ
                        if (MPERect[k].Y < rect.Center.Y)
                        {
                            angleFind = -angleFind;
                        }
                        if (angleMax > angleMin && angleFind < angleMax && angleFind > angleMin)
                        {
                            loodEnd = true;
                            endPoint = MPERect[k];
                            fDirect = true;
                        }
                        if (angleMax < angleMin && (angleFind < angleMax || angleFind > angleMin))
                        {
                            loodEnd = true;
                            endPoint = MPERect[k];
                            fDirect = true;
                        }
                        if (k == 3)
                        {
                            if(!fDirect)
                            {
                                //Tìm tâm của các contour con
                                Point pointCenterChild = new Point();
                                int numberChild = 0;
                                int indexChild = (int)arrHier.GetValue(0, indexCnt, 2);
                                while (indexChild != -1)
                                {
                                    if (CvInvoke.ContourArea(contours[indexChild]) > CvInvoke.ContourArea(contours[indexCnt]) * 0.1)
                                    {
                                        Moments momentChild = CvInvoke.Moments(contours[indexChild]);
                                        Point centerChild = new Point();
                                        centerChild.X = (int)(momentChild.M10 / momentChild.M00);
                                        centerChild.Y = (int)(momentChild.M01 / momentChild.M00);
                                        pointCenterChild.X += centerChild.X;
                                        pointCenterChild.Y += centerChild.Y;
                                        numberChild++;
                                    }
                                    indexChild = (int)arrHier.GetValue(0, indexChild, 0);
                                }

                                //Tìm trọng tâm khi có các contour con (nếu có)
                                Point centerPoint = center;
                                if (numberChild != 0)
                                {
                                    pointCenterChild.X = pointCenterChild.X / numberChild;
                                    pointCenterChild.Y = pointCenterChild.Y / numberChild;

                                    centerPoint.X = (int)(pointCenterChild.X + center.X) / 2;
                                    centerPoint.Y = (int)(pointCenterChild.Y + center.Y) / 2;
                                }
                                //Tìm trung điểm HCN có khoảng cách lớn nhất đến điểm trọng tâm
                                double distanceMax = 0;
                                int indexPTDMax = -1;
                                for (int n = 0; n < 4; n++)
                                {
                                    double distance = Math.Sqrt(Math.Pow(MPERect[n].X - centerPoint.X, 2) + Math.Pow(MPERect[n].Y - centerPoint.Y, 2));
                                    if (distance > distanceMax)
                                    {
                                        distanceMax = distance;
                                        indexPTDMax = n;
                                    }
                                }
                                directVec.X = MPERect[indexPTDMax].X - (int)rect.Center.X;
                                directVec.Y = MPERect[indexPTDMax].Y - (int)rect.Center.Y;
                                ///Vecto chỉ hướng của trục Ox là (1;0)
                                ///Tính cos của góc tạo bởi vecto hướng với Ox
                                CosAlpha = (double)directVec.X / Math.Sqrt(Math.Pow(directVec.X, 2) + Math.Pow(directVec.Y, 2));
                                ///Tính góc
                                angleFind = Math.Acos(CosAlpha) * 180 / Math.PI;
                                ///Fix về 360 độ
                                if (MPERect[indexPTDMax].Y < rect.Center.Y)
                                {
                                    angleFind = -angleFind;
                                }
                                fDirect = true;
                                endPoint = MPERect[indexPTDMax];
                            }

                            loodEnd = true;
                        }
                        k++;

                    }

                    var r1 = ConvertCoordinatesToOrigin(listROIFind[i].Location, center, angleFind, pointRectROI[0]);
                    var r2 = ConvertCoordinatesToOrigin(listROIFind[i].Location, center, angleFind, pointRectROI[1]);
                    var r3 = ConvertCoordinatesToOrigin(listROIFind[i].Location, center, angleFind, pointRectROI[2]);
                    var r4 = ConvertCoordinatesToOrigin(listROIFind[i].Location, center, angleFind, pointRectROI[3]);

                    CvInvoke.Line(imgOut, r1, r2, new MCvScalar(0, 255, 0), (int)ImgInput.Width / 170 + 1);
                    CvInvoke.Line(imgOut, r2, r3, new MCvScalar(0, 255, 0), (int)ImgInput.Width / 170 + 1);
                    CvInvoke.Line(imgOut, r3, r4, new MCvScalar(0, 255, 0), (int)ImgInput.Width / 170 + 1);
                    CvInvoke.Line(imgOut, r4, r1, new MCvScalar(0, 255, 0), (int)ImgInput.Width / 170 + 1);

                    
                    /// Vẽ ROI2 lên ảnh Out
                    if (listPointRectROI2.Count > 1 && numROI2 < 1)
                    {
                        var r11 = ConvertCoordinatesToOrigin(listROIFind[i].Location, center, angleFind, listPointRectROI2[0]);
                        var r12 = ConvertCoordinatesToOrigin(listROIFind[i].Location, center, angleFind, listPointRectROI2[1]);
                        var r13 = ConvertCoordinatesToOrigin(listROIFind[i].Location, center, angleFind, listPointRectROI2[2]);
                        var r14 = ConvertCoordinatesToOrigin(listROIFind[i].Location, center, angleFind, listPointRectROI2[3]);

                        CvInvoke.Line(imgOut, r11, r12, new MCvScalar(255, 255, 0), (int)ImgInput.Width / 170 + 1);
                        CvInvoke.Line(imgOut, r12, r13, new MCvScalar(255, 255, 0), (int)ImgInput.Width / 170 + 1);
                        CvInvoke.Line(imgOut, r13, r14, new MCvScalar(255, 255, 0), (int)ImgInput.Width / 170 + 1);
                        CvInvoke.Line(imgOut, r14, r11, new MCvScalar(255, 255, 0), (int)ImgInput.Width / 170 + 1);
                        numROI2++;
                    }


                    var pointOrigin = ConvertCoordinatesToOrigin(listROIFind[i].Location, center);
                    endPoint = ConvertCoordinatesToOrigin(listROIFind[i].Location, endPoint);
                    CvInvoke.ArrowedLine(imgOut, pointOrigin, endPoint, new MCvScalar(0, 255, 255), (int)imgOut.Width / 150, LineType.AntiAlias, 0, 0.1);
                    CvInvoke.Circle(imgOut, pointOrigin, 4, new MCvScalar(0, 0, 255), -1);

                    pointOrigin = ConvertCoordinatesToOrigin(listROIFind[i].Location, center);
                    Origin.Item1 = pointOrigin;
                    Origin.Item2 = angleFind;
                    listOrigin.Add(Origin);

                    
                }
                
            }



            numberFind = listImgROI.Count;
            if(listImgROI==null ) numberFind= 0;

                watch.Stop();
            var temp11 = watch.ElapsedMilliseconds;
            matchingTime = temp11.ToString();
        }


        /// <summary>
        /// Chuyển từ tọa độ của điểm trong hệ tọa độ bất kì về hệ tọa độ gốc ảnh với góc tương đối là 0
        /// </summary>
        /// <param name="StRoi">Tọa độ của gốc hệ tọa độ bất kì</param>
        /// <param name="point">Tọa độ của điểm cần chuyển trong hệ tọa độ đó</param>
        /// <returns></returns>
        private Point ConvertCoordinatesToOrigin(Point StRoi, Point point)
        {
            Point p = new Point();
            p.X = StRoi.X + point.X;
            p.Y = StRoi.Y + point.Y;
            return p;
        }


       
        /// <summary>
        /// Chuyển tọa độ của điểm từ hệ tọa độ gốc ảnh đến hệ tọa độ bất kì trong ROI
        /// </summary>
        /// <param name="StRoi">Tọa độ điểm đầu của ROI trong hệ tọa độ gốc ảnh</param>
        /// <param name="POrigin">Tọa độ của gốc tọa độ bất kì trong hệ tọa độ ROI</param>
        /// <param name="Angle">Góc tương đối giữa hệ tọa độ bất kì với hệ tọa độ gốc ảnh</param>
        /// <param name="point">Tọa độ của điểm cần chuyển</param>
        /// <returns></returns>
        private Point ConvertCoordinatesToNewOrigin(Point StRoi, Point POrigin, double Angle, Point point)
        {
            Point p = new Point();
            Angle = Angle * Math.PI / 180;
            var A = new Matrix<double>(new double[,] {
                { Math.Cos(Angle),-Math.Sin(Angle),POrigin.X + StRoi.X},
                { Math.Sin(Angle),Math.Cos(Angle),POrigin.Y + StRoi.Y},
                { 0,0,1} });
            var B = new Matrix<double>(new double[,]
            {
                {point.X },
                {point.Y},
                {1 }
            });
            var AT = new Matrix<double>(3, 1);
            CvInvoke.Invert(A, AT, DecompMethod.LU);
            var C = new Matrix<double>(3, 1);
            CvInvoke.Gemm(AT, B, 1.0, null, 0, C);
            p.X = (int)C.Data[0, 0];
            p.Y = (int)C.Data[1, 0];
            return p;
        }


        /// <summary>
        /// Chuyển tọa độ của ảnh từ hệ tọa độ bất kì về hệ tọa độ gốc ảnh
        /// </summary>
        /// <param name="Coordinates">Hệ tọa độ bất kì trong hệ tọa độ gốc ảnh</param>
        /// <param name="point">Tọa độ điểm trong hệ tọa độ bất kì cần chuyển về tọa độ gốc</param>
        /// <returns></returns>
        private Point ConvertCoordinatesToOrigin(ValueTuple<Point,double> Coordinates, Point point)
        {
            Point p = new Point();
            var Angle = Coordinates.Item2 * Math.PI / 180;
            var A = new Matrix<double>(new double[,] {
                { Math.Cos(Angle),-Math.Sin(Angle),Coordinates.Item1.X},
                { Math.Sin(Angle),Math.Cos(Angle),Coordinates.Item1.Y},
                { 0,0,1} });
            var B = new Matrix<double>(new double[,]
            {
                {point.X },
                {point.Y},
                {1 }
            });
            var C = new Matrix<double>(3, 1);
            CvInvoke.Gemm(A, B, 1.0, null, 0, C);
            p.X = (int)C.Data[0, 0];
            p.Y = (int)C.Data[1, 0];
            return p;
        }



        private Point ConvertCoordinatesToOrigin(Point StRoi, Point POrigin, double Angle, Point point)
        {
            Point p = new Point();
            Angle = Angle * Math.PI / 180;
            //
            var A = new Matrix<double>(new double[,] {
                { Math.Cos(Angle),-Math.Sin(Angle),POrigin.X + StRoi.X},
                { Math.Sin(Angle),Math.Cos(Angle),POrigin.Y + StRoi.Y},
                { 0,0,1} });
            var B = new Matrix<double>(new double[,]
            {
                {point.X },
                {point.Y},
                {1 }
            });

            var C = new Matrix<double>(3, 1);
            CvInvoke.Gemm(A, B, 1.0, null, 0, C);
            p.X = (int)C.Data[0, 0];
            p.Y = (int)C.Data[1, 0];
            return p;
        }


        private void testCuda()
        {
            Mat imgout = new Mat();
            CudaTemplateMatching matcher = new CudaTemplateMatching(DepthType.Default, 3, TemplateMatchingType.CcoeffNormed);
            
            matcher.Match(imgInput, imgTemplate, imgout);
            double minval = 0;
            double maxval = 0;
            Point minloc = new Point();
            Point maxloc = new Point();
            

            CvInvoke.MinMaxLoc(imgout, ref minval, ref maxval, ref minloc, ref maxloc);
        }
        public void CUDACheck()
        {
            if (CudaInvoke.HasCuda)
            {
                MessageBox.Show("CUDA On");
            }
            else
            {
                MessageBox.Show("CUDA Off");
            }


        }
        private void button1_Click(object sender, EventArgs e)
        {
            CUDACheck();
            

        }

        
    }
}
