using System;
using System.Collections.Generic;
using System.Text;

namespace SprayPaintApp.Data
{
    public class Ink
    {
        public int Id { get; set; }
        public byte[] CanvasByte { get; set; }
        public DateTimeOffset CreatedUtc { get; set; }

    }
}
