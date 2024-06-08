public class Bucket : BaseTool
{
	public bool IsFilled;

	public Bucket() : base(new Water()) {}

	public override bool CanUse() => IsFilled;
}