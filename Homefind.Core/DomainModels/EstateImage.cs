﻿namespace Homefind.Core.DomainModels
{
    public class EstateImage : BaseEntity
    {
        public long EstateUnitId { get; set; }

        public string Name { get; set; }

        public byte[] Data { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public string ContentType { get; set; }
    }
}
