using Siterm.Instructions.Models;
using Siterm.Signature.Resources;

namespace Siterm.Signature.Models
{
    public class SignatureBundle
    {
        public readonly SignatureCause Cause;
        public readonly UserDraft UserDraft;

        public SignatureBundle(UserDraft userDraft, SignatureCause cause)
        {
            UserDraft = userDraft;
            Cause = cause;
        }
    }
}