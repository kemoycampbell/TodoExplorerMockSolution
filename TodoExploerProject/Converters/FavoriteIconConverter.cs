using System.Globalization;

namespace TodoExploerProject.Converters;

public class FavoriteIconConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        bool isFavorite = (bool)value;
        if(isFavorite)
            return "favorite.png";
        return "not_favorite.png";
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        string icon = (string)value;
        if(icon == "favorite.png")
            return true;
        return false;
    }
}