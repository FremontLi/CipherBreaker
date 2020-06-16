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

                char[] str = new char[fi.Length];
                for(int i = 0; i < fi.Length; i++)
                {
                    str[i] = (char)buff[i];
                }
                string strTemp = new string(str);

                Scheme railFence = Scheme.ChooseScheme(plain: strTemp, cipher: null, key: "4");
                (var cipher, _) = railFence.Encode();


                byte[] cipherByte = new byte[fi.Length];
                char[] cipherChar = cipher.ToCharArray();
                for(int i = 0; i < fi.Length; i++)
                {
                    cipherByte[i] = (byte)cipherChar[i];
                }

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

                char[] str = new char[fi.Length];
                for (int i = 0; i < fi.Length; i++)
                {
                    str[i] = (char)buff[i];
                }
                string strTemp = new string(str);

                Scheme railFence = Scheme.ChooseScheme(plain: null, cipher: strTemp, key: "4");
                (var plain, _) = railFence.Decode();

                byte[] plainByte = new byte[fi.Length];
                char[] plainChar = plain.ToCharArray();
                for (int i = 0; i < fi.Length; i++)
                {
                    plainByte[i] = (byte)plainChar[i];
                }

                if (File.Exists(savepath))
                {
                    File.Delete(savepath);
                }

                FileStream decodersave = new FileStream(savepath, FileMode.CreateNew);
                BinaryWriter bw = new BinaryWriter(decodersave);
                bw.Write(plainByte, 0, plainByte.Length);
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
