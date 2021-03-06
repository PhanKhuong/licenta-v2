﻿using System.Collections.Generic;
using Homefind.Core.DomainModels;

namespace Homefind.Web.Models.PropertyViewModels
{
    public class DetailsViewModel : BaseViewModel
    {
        public EstateUnit Property { get; set; }

        public IEnumerable<PropertyInfoModel> Popular { get; set; }
    }
}
