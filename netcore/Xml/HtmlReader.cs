using System;
using System.IO;
using System.Text;
using System.Xml;

namespace netcore.Xml
{
    public sealed class HtmlReader : XmlReader
    {
        private Stream stream;
        private ReadState readState;
        private XmlNode currentNode;
        private byte[] buffer;
        private int bytesUsed;
        private long position;

        public HtmlReader(Stream stream)
        {
            this.stream = stream;
            readState = ReadState.Initial;

            buffer = new byte[100];
            bytesUsed = 0;

            Console.WriteLine("start reading");
            int read = stream.Read(buffer, bytesUsed, 100);

            Encoding encoding = DetectEncoding();
            byte[] preamble = encoding.GetPreamble();
            Console.WriteLine("the preamble length is " + preamble.Length);
            position = preamble.Length;

            byte[] b2 = new byte[100 - preamble.Length];

            Array.Copy(buffer, preamble.Length, b2, 0, b2.Length);

            Console.WriteLine(encoding.GetString(b2));
            currentNode = null;
        }

        private Encoding DetectEncoding()
        {
            Console.WriteLine("detect encoding");

            Encoding encoding = null;

            int first2bytes = buffer[0] << 8 | buffer[1];

            Console.WriteLine(string.Format("first byte {0:x16}", buffer[0]));
            Console.WriteLine(string.Format("second byte {0:x16}", buffer[1]));
            Console.WriteLine(string.Format("together {0:x16}", first2bytes));
            switch (first2bytes)
            {
                case 0xFEFF:
                case 0x003C:
                    encoding = Encoding.BigEndianUnicode;
                    break;
                case 0xFFFE:
                case 0x3C00:
                    encoding = Encoding.Unicode;
                    break;
                case 0xEFBB:
                    encoding = Encoding.UTF8;
                    break;
                default:
                    break;
            }

            return encoding;
        }

        public override bool MoveToFirstAttribute()
        {
            throw new NotImplementedException();
        }

        public override string NamespaceURI
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override ReadState ReadState
        {
            get
            {
                return readState;
            }
        }

        public override int Depth
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override bool IsEmptyElement
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override string GetAttribute(int i)
        {
            throw new NotImplementedException();
        }

        public override string Value
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override bool MoveToNextAttribute()
        {
            throw new NotImplementedException();
        }

        public override void MoveToAttribute(int i)
        {
            base.MoveToAttribute(i);
        }

        public override bool EOF
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override string LookupNamespace(string prefix)
        {
            throw new NotImplementedException();
        }

        public override string GetAttribute(string name)
        {
            throw new NotImplementedException();
        }

        public override int AttributeCount
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override string LocalName
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override string GetAttribute(string name, string namespaceURI)
        {
            throw new NotImplementedException();
        }

        public override XmlNameTable NameTable
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override void ResolveEntity()
        {
            throw new NotImplementedException();
        }

        public override bool ReadAttributeValue()
        {
            throw new NotImplementedException();
        }

        public override bool MoveToElement()
        {
            throw new NotImplementedException();
        }

        public override bool Read()
        {
            throw new NotImplementedException();
        }

        public override bool MoveToAttribute(string name)
        {
            throw new NotImplementedException();
        }

        public override string BaseURI
        {
            get
            {
                return "http://mockuri.com";
            }
        }

        public override string Prefix
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override XmlNodeType NodeType
        {
            get
            {
                return currentNode.NodeType;
            }
        }

        public override bool MoveToAttribute(string name, string ns)
        {
            throw new NotImplementedException();
        }
    }
}