//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Gallery.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Comment
    {
        public int CommentId { get; set; }
        public string Text { get; set; }
        public int ImageId { get; set; }
        public Nullable<int> UserId { get; set; }
    
        public virtual Image Image { get; set; }
        public virtual User User { get; set; }
    }
}
