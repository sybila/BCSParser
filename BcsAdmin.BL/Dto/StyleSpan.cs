using TextRange = BcsResolver.Syntax.Tokenizer.TextRange;

namespace BcsAdmin.BL.Dto
{
    public class StyleSpan
    {
        public TextRange Range { get; set; }
        public string CssClass { get; set; }
    }
}