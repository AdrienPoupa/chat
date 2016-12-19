using System;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace Server.Views
{
    /// <summary>
    /// Class used to write Console output to a WinForm
    /// Source: https://saezndaree.wordpress.com/2009/03/29/how-to-redirect-the-consoles-output-to-a-textbox-in-c/
    /// </summary>
    public class TextBoxStreamWriter : TextWriter
    {
        TextBox _output = null;

        public TextBoxStreamWriter(TextBox output)
        {
            _output = output;
        }

        public override void Write(char value)
        {
            base.Write(value);
            // thread safe text appending source: http://stackoverflow.com/a/10804951
            // multi line appending source: http://stackoverflow.com/questions/8536958/how-to-add-a-line-to-a-multiline-textbox
            try
            {
                _output.Invoke((MethodInvoker) (() =>
                {
                    _output.AppendText(value.ToString());
                }));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
        }

        public override Encoding Encoding
        {
            get { return System.Text.Encoding.UTF8; }
        }
    }
 }