using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace BBS2018.Web
{
    /// <summary>
    /// 验证码生成器
    /// </summary>
    public class VerifyCodeHelper
    {

        #region 生成验证码
        /// <summary>
        /// 生成验证码
        /// </summary>
        /// <param name="length">指定验证码的长度</param>
        /// <returns></returns>
        public string CreateVerifyCode(int length)
        {
            //int[] randMembers = new int[length];
            //int[] validateNums = new int[length];
            //string validateNumberStr = "";
            ////生成起始序列值
            //int seekSeek = unchecked((int)DateTime.Now.Ticks);
            //Random seekRand = new Random(seekSeek);
            //int beginSeek = (int)seekRand.Next(0, Int32.MaxValue - length * 10000);
            //int[] seeks = new int[length];
            //for (int i = 0; i < length; i++)
            //{
            //    beginSeek += 10000;
            //    seeks[i] = beginSeek;
            //}
            ////生成随机数字
            //for (int i = 0; i < length; i++)
            //{
            //    Random rand = new Random(seeks[i]);
            //    int pownum = 1 * (int)Math.Pow(10, length);
            //    randMembers[i] = rand.Next(pownum, Int32.MaxValue);
            //}
            ////抽取随机数字
            //for (int i = 0; i < length; i++)
            //{
            //    string numStr = randMembers[i].ToString();
            //    int numLength = numStr.Length;
            //    Random rand = new Random();
            //    int numPosition = rand.Next(0, numLength - 1);
            //    validateNums[i] = Int32.Parse(numStr.Substring(numPosition, 1));
            //}

            //生成验证码
            //for (int i = 0; i < length; i++)
            //{
            //    validateNumberStr += validateNums[i].ToString();
            //}
            //return validateNumberStr;

            Random random = new Random();
            string randomCode = "";

            for (int i = 0; i < length; i++)
            {
                randomCode += random.Next(0, 9).ToString();
            }

            return randomCode;
        }
        #endregion

        #region 创建验证码图片
        public byte[] CreateGraphic(string code)
        {
            Bitmap image = new Bitmap((int)Math.Ceiling(code.Length * 12.0), 22);
            Graphics g = Graphics.FromImage(image);

            try
            {
                //清空图片背景
                g.Clear(Color.White);

                Font font = new Font("Arial", 12, (FontStyle.Bold | FontStyle.Italic));
                LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height),
Color.Blue, Color.DarkRed, 1.2f, true);
                g.DrawString(code, font, brush, 3, 2);

                //画边框线
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);



                MemoryStream stream = new MemoryStream();
                image.Save(stream, ImageFormat.Jpeg);

                return stream.ToArray();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }
        #endregion

    }
}