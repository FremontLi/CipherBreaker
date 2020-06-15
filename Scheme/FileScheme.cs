using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CipherBreaker
{
    /// <summary>
    /// �����ࣺ�ļ�������������ת��
    /// </summary>
    class FileScheme
    {
        /// <summary>
        /// ���ļ�ת��Ϊbyte���鲢����
        /// </summary>
        /// <param name="path">�ļ���ַ</param>
        /// <returns>ת�������ܺ��byte����</returns>
        public void File2Bytes(string path, string savepath)
        {
            try
            {
                FileInfo fi = new FileInfo(path);       //���������ļ�
                byte[] buff = new byte[fi.Length];      //�����ֽ����鳤��

                FileStream fs = fi.OpenRead();
                fs.Read(buff, 0, Convert.ToInt32(fs.Length));
                fs.Close();


                string strTemp;
                strTemp = System.Text.Encoding.Default.GetString(buff);

                Scheme railFence = Scheme.ChooseScheme(plain: strTemp, cipher: null, key: "4");
                (var cipher, _) = railFence.Encode();
                //RailFence railFence = new RailFence(plain: strTemp, key: "4");
                //(var cipher, _) = railFence.Encode();
                byte[] cipherByte = System.Text.Encoding.Default.GetBytes(cipher);


                if (File.Exists(savepath))
                {
                    File.Delete(savepath);
                }

                FileStream decodingSave = new FileStream(savepath, FileMode.CreateNew);
                BinaryWriter bw = new BinaryWriter(decodingSave);
                bw.Write(cipherByte, 0, cipherByte.Length);
                bw.Close();
                decodingSave.Close();
            }
            catch(Exception e)
            {
                Console.WriteLine("Exception:"+e.ToString());
            }
        }

            /// <summary>
            /// ��byte������ܲ�ת��Ϊ�ļ������浽ָ����ַ
            /// </summary>
            /// <param name="buff">Ҫ���ܵ�byte����</param>
            /// <param name="savepath">�����ַ</param>
        public void Bytes2File(string path, string savepath)
        {
            try
            {
                FileInfo fi = new FileInfo(path);       //���������ļ�
                byte[] buff = new byte[fi.Length];      //�����ֽ����鳤��

                FileStream fs = fi.OpenRead();
                fs.Read(buff, 0, Convert.ToInt32(fs.Length));
                fs.Close();


                string strTemp;
                strTemp = System.Text.Encoding.Default.GetString(buff);
                Scheme railFence = Scheme.ChooseScheme(plain: null, cipher: strTemp, key: "4");
                (var plain, _) = railFence.Decode();
                //RailFence railFence = new RailFence(cipher: strTemp, key: "4");
                //(var plain, _) = railFence.Decode();
                byte[] cipherByte = System.Text.Encoding.Default.GetBytes(plain);



                if (File.Exists(savepath))
                {
                    File.Delete(savepath);
                }

                FileStream decodersave = new FileStream(savepath, FileMode.CreateNew);
                BinaryWriter bw = new BinaryWriter(decodersave);
                bw.Write(cipherByte, 0, cipherByte.Length);
                bw.Close();
                decodersave.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception:" + e.ToString());
            }
        }
    }    
}
