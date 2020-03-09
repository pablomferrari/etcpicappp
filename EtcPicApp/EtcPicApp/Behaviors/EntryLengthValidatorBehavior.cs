using Xamarin.Forms;

namespace EtcPicApp.Behaviors
{
    public class EntryLengthValidatorBehavior : Behavior<Entry>
    {
        public int MaxLength { get; set; }
        public int MinLength { get; set; }
        protected override void OnAttachedTo(Entry bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.TextChanged += OnEntryTextChanged;
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.TextChanged -= OnEntryTextChanged;
        }

        void OnEntryTextChanged(object sender,  TextChangedEventArgs e)
        {
            var entry = (Entry)sender;

            if (entry.Text.Length < MinLength)
            {
                entry.Text = 0.ToString();
            }
        }
    }
}
