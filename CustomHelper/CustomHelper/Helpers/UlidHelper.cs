namespace CustomHelper.Helpers
{
    public static class UlidHelper
    {
        public static Ulid ParseUlid(this string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return Ulid.NewUlid();
            }

            _ = Ulid.TryParse(id, out Ulid res);
            return res;
        }
    }
}
