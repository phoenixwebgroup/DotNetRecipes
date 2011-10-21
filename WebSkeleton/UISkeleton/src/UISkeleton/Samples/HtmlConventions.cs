namespace UISkeleton.Samples
{
	using FubuMVC.UI.Configuration;
	using HtmlTags;
	using HtmlTags.Constants;
	using HtmlTags.UI;
	using HtmlTags.UI.Attributes;
	using HtmlTags.UI.AutoComplete;
	using HtmlTags.UI.Builders;

	public class HtmlConventions : HtmlConventionsRegistryBase
	{
		public HtmlConventions()
		{
			RegisterBuilders();
			RegisterDefaults();
			RegisterModifiers();
		}

		private void RegisterBuilders()
		{
			Editors.IfPropertyTypeIsEnum().BuildBy(new EnumDropDownBuilder().Build);
			Editors.IfPropertyTypeIsNullableEnum().BuildBy(new EnumDropDownBuilder().Build);

			Editors.Builder<HiddenInputBuilder>();
			Editors.Builder<MultiSelectListBuilder>();
			Editors.Builder<AjaxSelectListBuilder>();
			Editors.Builder<SelectListBuilder>();
			Editors.Builder<CheckBoxListBuilder>();
			Editors.Builder<CheckBoxBuilder>();
			Editors.Builder<DateAndTimePickerBuilder>();
			Editors.Builder<DatePickerBuilder>();
			Editors.Builder<PasswordBuilder>();
			Editors.Builder<TextAreaBuilder>();
			Editors.Builder<AutoCompleteBuilder>();
			Editors.IfPropertyHasAttribute<ClickToEditAttribute>().Modify(ClickToEditModifier.ClickToEdit);

			// todo might want a custom numeric display builder with rounding as your application desires
			//Displays.Builder<NumericDisplayBuilder>();
			Displays.Builder<HiddenDisplayBuilder>();
			Displays.Builder<DateOnlyBuilder>();
			Displays.Builder<YesOrNoBuilder>();
			Displays.Builder<FlagsEnumDisplayBuilder>();

			Labels.Builder<HiddenLabelBuilder>();

			RegisterValidationModifiers();
		}

		private void RegisterModifiers()
		{
			Editors.Always.Modify(AddElementName);
		}

		private void RegisterDefaults()
		{
			Displays.Always.BuildBy(DefaultDisplay.Builder);
			Labels.Always.BuildBy(DefaultLabeler.Default);
			Editors.Always.BuildBy(DefaultEditor.Default);
		}

		public static void AddElementName(ElementRequest request, HtmlTag tag)
		{
			if (tag.IsInputElement())
			{
				tag.Attr(HtmlAttributeConstants.Name, request.ElementId);
			}
		}
	}
}