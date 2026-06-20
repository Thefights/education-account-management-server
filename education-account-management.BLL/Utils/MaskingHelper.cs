using System.Text.Json;
using System.Text.Json.Nodes;

namespace Utils
{
    public static class MaskingHelper
    {
        public static string? MaskPayload(string? rawJson)
        {
            if (string.IsNullOrWhiteSpace(rawJson)) return rawJson;
            var normalizedJson = NormalizeJsonPayload(rawJson);
            var node = JsonNode.Parse(normalizedJson!);
            if (node is JsonObject obj)
            {
                MaskObject(obj);
                return node.ToJsonString();
            }
            return normalizedJson;
        }

        public static string? NormalizeJsonPayload(string? rawJson)
        {
            if (string.IsNullOrWhiteSpace(rawJson)) return rawJson;

            try
            {
                using var document = JsonDocument.Parse(rawJson);
                return rawJson;
            }
            catch (JsonException)
            {
                return JsonSerializer.Serialize(new { Message = rawJson });
            }
        }

        private static void MaskObject(JsonObject obj)
        {
            var nricKeys = obj.Where(p => string.Equals(p.Key, "nric", StringComparison.OrdinalIgnoreCase))
                             .Select(p => p.Key)
                             .ToList();

            foreach (var key in nricKeys)
            {
                var val = obj[key]?.GetValue<string>();
                if (!string.IsNullOrEmpty(val))
                {
                    obj[key] = MaskNric(val);
                }
            }

            foreach (var property in obj)
            {
                if (property.Value is JsonObject childObj)
                {
                    MaskObject(childObj);
                }
                else if (property.Value is JsonArray childArray)
                {
                    foreach (var item in childArray)
                    {
                        if (item is JsonObject arrayObj)
                        {
                            MaskObject(arrayObj);
                        }
                    }
                }
            }
        }

        public static string MaskNric(string nric)
        {
            if (string.IsNullOrWhiteSpace(nric)) return string.Empty;
            var trimmed = nric.Trim();
            if (trimmed.Length < 5) return trimmed;

            var firstChar = trimmed[0];
            var lastFour = trimmed[^4..];

            return $"{firstChar}****{lastFour}";
        }

    }
}
