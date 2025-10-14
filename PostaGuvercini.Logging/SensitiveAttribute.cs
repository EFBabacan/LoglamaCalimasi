using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PostaGuvercini.Logging
{
    /// <summary>
    /// Bir property'nin hassas veri içerdiğini ve loglarda maskelenmesi gerektiğini işaretler.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class SensitiveAttribute : Attribute
    {
    }
}