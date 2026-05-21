namespace SandFall
{
    public class PixelContainer
    {
        private Pixel? _pixel;

        public Pixel? Pixel { get => _pixel; set => _pixel = value; }
        public bool IsEmpty => !_pixel.HasValue;
    }
}
