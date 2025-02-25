﻿using System;
using System.IO;

namespace TestTask
{
    public class ReadOnlyStream : IReadOnlyStream
    {
        private StreamReader _localStream;

        /// <summary>
        /// Конструктор класса. 
        /// Т.к. происходит прямая работа с файлом, необходимо 
        /// обеспечить ГАРАНТИРОВАННОЕ закрытие файла после окончания работы с таковым!
        /// </summary>
        /// <param name="fileFullPath">Полный путь до файла для чтения</param>
        public ReadOnlyStream(string fileFullPath)
        {
            IsEof = true;

            _localStream = new StreamReader(fileFullPath);
        }
                
        /// <summary>
        /// Флаг окончания файла.
        /// </summary>
        public bool IsEof
        {
            get;
            private set;
        }

        /// <summary>
        /// Ф-ция чтения следующего символа из потока.
        /// Если произведена попытка прочитать символ после достижения конца файла, метод 
        /// должен бросать соответствующее исключение
        /// </summary>
        /// <returns>Считанный символ.</returns>
        public char ReadNextChar()
        {
            try {
                if (IsEof == true || _localStream.Peek() == -1)
                {
                    throw new ArgumentOutOfRangeException();
                }
                char result = (char)_localStream.Read();
                if (_localStream.Peek() == -1)
                {
                    IsEof = true;
                }
                return result;
            }
            catch (ArgumentOutOfRangeException) {
                IsEof = true;
                throw new ArgumentOutOfRangeException();
            }
            catch (Exception ex) {
                IsEof = true;
                throw new NotImplementedException(ex.Message);
            }
        }

        /// <summary>
        /// Сбрасывает текущую позицию потока на начало.
        /// </summary>
        public void ResetPositionToStart()
        {
            if (_localStream == null)
            {
                IsEof = true;
                return;
            }

            _localStream.DiscardBufferedData();
            IsEof = false;
        }

        public void Dispose ()
        {
            _localStream.Dispose();
        }

        ~ReadOnlyStream()
        {
            Dispose();
        }
    }
}
