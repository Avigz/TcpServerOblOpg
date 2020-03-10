using System;
using System.Collections.Generic;
using System.Text;

namespace TcpServerOblOpg
{
    public class Book
    {
        private string _titel;
        private string _forfatter;
        private int _sidetal;
        private string _isbn13;

        public Book(string title, string forfatter, int sidetal, string isbn13)
        {
            if (isbn13.Length == 13)
            {
                _isbn13 = isbn13;
            }
            else if (isbn13.Length != 13)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (sidetal < 4 || sidetal > 1000)
            {
                throw new ArgumentOutOfRangeException();
            }
            else
            {
                _sidetal = sidetal;
            }

            if (forfatter.Length >= 2)
            {
                _forfatter = forfatter;
            }
            else if (forfatter.Length < 2) { throw new ArgumentOutOfRangeException(); }

            _titel = title;
        }

        public string ISBN13
        {
            get => _isbn13;

            set
            {
                if (value.Length == 13)
                { _isbn13 = value; }

                else { throw new ArgumentOutOfRangeException(); }
            }
        }

        public int Sidetal
        {
            get => _sidetal;
            set
            {
                if (value >= 4 && value <= 1000)
                { _sidetal = value; }
                else { throw new ArgumentOutOfRangeException(); }
            }
        }

        public string Forfatter
        {
            get => _forfatter;
            set
            {
                if (value.Length >= 2)
                { _forfatter = value; }
                else { throw new ArgumentOutOfRangeException(); }
            }
        }

        public string Titel
        {
            get => _titel;
            set => _titel = value;
        }

        public override string ToString()
        {
            return "" + Titel + " " + Forfatter + " " + Sidetal + " " + ISBN13;
        }
    }
}
