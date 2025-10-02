namespace GW.Demo.Maui.Selectors;



public class ScreenTemplateSelector: DataTemplateSelector
{
    public DataTemplate? PositionTemplate { get; set; }
    public DataTemplate? PostsTemplate { get; set; }
    public DataTemplate? VideoTemplate { get; set; }
    public DataTemplate? PhotoTemplate { get; set; }
    public DataTemplate? ContactsTemplate { get; set; }


    protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
    {
        if(item is int i)
        {
            return i switch
            {
                0 => PositionTemplate!,
                1 => PostsTemplate!,
                2 => VideoTemplate!,
                3 => PhotoTemplate!,
                4 => ContactsTemplate!,
                _ => PositionTemplate!
            };
        }

        return PositionTemplate!;
    }
}
