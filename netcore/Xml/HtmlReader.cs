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
        private byte[] buffer;
        private Encoding encoding;
        private int bytesUsed;
        private long position;
        private XmlNodeType nodeType;
        private bool isEmptyElement;
        private string localName;
        private string attrValue;

        public HtmlReader(Stream stream)
        {
            this.stream = stream;
            readState = ReadState.Initial;

            buffer = new byte[stream.Length];
            bytesUsed = 0;

            Console.WriteLine("start reading");
            int read = stream.Read(buffer, bytesUsed, buffer.Length);

            encoding = DetectEncoding();
            byte[] preamble = encoding.GetPreamble();
            Console.WriteLine("the preamble length is " + preamble.Length);
            position = preamble.Length;


            for (long i = position; i < buffer.Length; i++)
            {
                char c = (char)buffer[i];
                if (c == '>')
                {
                    position = i;
                    break;
                }
            }

            position++;

            long localNameOffset = position;
            long localNameEnd = 0;
            for (long i = position; i < buffer.Length; i++)
            {
                char c = (char)buffer[i];
                if (c == '<')
                {
                    localNameOffset = i + 1;
                }

                if (c == ' ')
                {
                    localNameEnd = i - 1;
                    position = i;
                    break;
                }
            }

            position++;

            byte[] tagArray = new byte[localNameEnd - localNameOffset + 1];

            Array.Copy(buffer, (int)localNameOffset, tagArray, 0, tagArray.Length);
            Console.WriteLine(encoding.GetString(tagArray));
            localName = encoding.GetString(tagArray);

            nodeType = XmlNodeType.Element;
            isEmptyElement = false;

            byte[] b2 = new byte[100];

            Array.Copy(buffer, (int)position, b2, 0, b2.Length);



            Console.WriteLine(encoding.GetString(b2));
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
            Console.Write("MoveToFirstAttribute");
            bool result = true;

            char c = (char)buffer[position];
            if (c == '>')
            {
                result = false;
            }
            else
            {
                nodeType = XmlNodeType.Attribute;

                long offset = position;
                long end = 0;
                for (long i = position; i < buffer.Length; i++)
                {
                    c = (char)buffer[i];
                    if (c == '=')
                    {
                        end = i;
                        position = i;

                        byte[] attrArray = new byte[end - offset];
                        Array.Copy(buffer, (int)offset, attrArray, 0, attrArray.Length);
                        localName = encoding.GetString(attrArray);
                        Console.Write("the attribute name is " + localName);
                        break;
                    }
                }
                position++;
            }

            return result;
        }

        public override string NamespaceURI
        {
            get
            {
                Console.Write("get namespaceURI");
                return string.Empty;
            }
        }

        public override ReadState ReadState
        {
            get
            {
                Console.Write("get read state");
                return readState;
            }
        }

        public override int Depth
        {
            get
            {
                Console.Write("get depth");
                throw new NotImplementedException();
            }
        }

        public override bool IsEmptyElement
        {
            get
            {
                Console.Write("get isemptyelement");
                return isEmptyElement;
            }
        }

        public override string GetAttribute(int i)
        {
            Console.Write("GetAttribute");
            throw new NotImplementedException();
        }

        public override string Value
        {
            get
            {
                Console.Write("get value " + attrValue);
                return attrValue;
            }
        }

        public override bool MoveToNextAttribute()
        {
            Console.Write("MoveToNextAttribute");

            bool result = false;

            nodeType = XmlNodeType.Attribute;

            long offset = position;
            long end = 0;
            bool isFirstNonSpace = false;
            for (long i = position; i < buffer.Length; i++)
            {
                char c = (char)buffer[i];

                if (c == '>')
                {
                    nodeType = XmlNodeType.Element;
                    result = false;
                    position = i + 1;
                    break;
                }
                if (c != ' ' && !isFirstNonSpace)
                {
                    offset = i;
                    isFirstNonSpace = true;
                }

                if (c == '=')
                {
                    end = i - 1;
                    position = i;

                    byte[] attrArray = new byte[end - offset + 1];
                    Array.Copy(buffer, (int)offset, attrArray, 0, attrArray.Length);
                    localName = encoding.GetString(attrArray);
                    Console.Write("the attribute name is " + localName);
                    position++;

                    result = true;
                    break;
                }
            }

            return result;
        }

        public override void MoveToAttribute(int i)
        {
            Console.Write("MoveToAttribute");
            base.MoveToAttribute(i);
        }

        public override bool EOF
        {
            get
            {
                Console.Write("get EOF");
                throw new NotImplementedException();
            }
        }

        public override string LookupNamespace(string prefix)
        {
            Console.Write("LookupNamespace");
            throw new NotImplementedException();
        }

        public override string GetAttribute(string name)
        {
            Console.Write("GetAttribute by name");
            throw new NotImplementedException();
        }

        public override int AttributeCount
        {
            get
            {
                Console.Write("get AttributeCount");
                throw new NotImplementedException();
            }
        }

        public override string LocalName
        {
            get
            {
                Console.Write("get LocalName: " + localName);
                return localName;
            }
        }

        public override string GetAttribute(string name, string namespaceURI)
        {
            Console.Write("GetAttribute by name & namespaceuri");
            throw new NotImplementedException();
        }

        public override XmlNameTable NameTable
        {
            get
            {
                Console.Write("get NameTable");
                throw new NotImplementedException();
            }
        }

        public override void ResolveEntity()
        {
            Console.Write("ResolveEntity");
            throw new NotImplementedException();
        }

        public override bool ReadAttributeValue()
        {
            Console.Write("ReadAttributeValue");

            bool result = false;

            long offset = 0;
            long end = 0;
            bool isStarted = false;

            for (long i = position; i < buffer.Length; i++)
            {
                char c = (char)buffer[i];
                // Console.Write("ReadAttributeValue c is " + c + ".");

                if (c == ' ')
                {
                    if (!isStarted)
                    {
                        result = false;
                        position = i + 1;
                        break;
                    }
                }

                if (c == '>')
                {
                    nodeType = XmlNodeType.Element;
                    result = false;
                    position = i + 1;
                    break;
                }

                if (c == '"')
                {
                    if (isStarted)
                    {
                        end = i - 1;
                        position = i + 1;
                        byte[] valueArray = new byte[end - offset + 1];
                        Array.Copy(buffer, (int)offset, valueArray, 0, valueArray.Length);
                        attrValue = encoding.GetString(valueArray);

                        nodeType = XmlNodeType.Text;
                        Console.WriteLine("the attribute value is " + attrValue);
                        result = true;
                        break;
                    }
                    else
                    {
                        offset = i + 1;
                        isStarted = true;
                    }
                }
            }

            return result;
        }

        public override bool MoveToElement()
        {
            Console.Write("MoveToElement");

            // nodeType = XmlNodeType.Element;

            return true;
        }

        public override bool Read()
        {
            Console.Write("Read");

            long offset = 0;
            long end = 0;
            bool isStarted = false;
            for (long i = position; i < buffer.Length; i++)
            {
                char c = (char)buffer[i];

                if (c == '<')
                {
                    nodeType = XmlNodeType.Element;
                    offset = i + 1;
                    isStarted = true;
                }

                if (c == ' ' && isStarted)
                {
                    end = i - 1;
                    byte[] tagArray = new byte[end - offset + 1];
                    Array.Copy(buffer, (int)offset, tagArray, 0, tagArray.Length);
                    localName = encoding.GetString(tagArray);
                    Console.WriteLine("the localname is " + localName);
                    position = i + 1;
                    break;
                }
            }

            return true;
        }

        public override bool MoveToAttribute(string name)
        {
            Console.Write("MoveToAttribute by name");
            throw new NotImplementedException();
        }

        public override string BaseURI
        {
            get
            {
                Console.Write("get BaseURI");
                return "http://mockuri.com";
            }
        }

        public override string Prefix
        {
            get
            {
                Console.Write("get Prefix");
                return string.Empty;
            }
        }

        public override XmlNodeType NodeType
        {
            get
            {
                Console.Write("get NodeType: " + nodeType);
                return nodeType;
            }
        }

        public override bool MoveToAttribute(string name, string ns)
        {
            Console.Write("MoveToAttribute by name & ns");
            throw new NotImplementedException();
        }
    }
}