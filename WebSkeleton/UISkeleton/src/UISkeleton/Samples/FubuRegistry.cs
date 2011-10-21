namespace UISkeleton.Samples
{
	using Castle.Windsor;
	using FubuCore;
	using FubuMVC.UI;
	using FubuMVC.UI.Configuration;
	using FubuMVC.UI.Tags;
	using GotFour.Windsor;
	using HtmlTags.UI;
	using HtmlTags.UI.Builders;
	using HtmlTags.UI.Conventions;
	using HtmlTags.UI.Helpers;
	using Infrastructure;

	public class FubuRegistry : ExtendedRegistryBase, IRunOnApplicationStart
	{
		public FubuRegistry()
		{
			// todo plug in HtmlConventions implementation
			AddComponent<HtmlConventionRegistry, HtmlConventions>();
			AddComponent<IElementNamingConvention, DottedElementNamingConvention>();
			For<Stringifier>();
			For<TagProfileLibrary>().LifeStyle.Singleton();
			For(typeof (ITagGenerator<>)).ImplementedBy(typeof (TagGenerator<>));
			AddComponent<ITypeResolver, TypeResolver>();
		}

		public void Start(IWindsorContainer container)
		{
			var library = container.Resolve<TagProfileLibrary>();
			var conventions = container.Resolve<HtmlConventionRegistry>();
			library.ImportRegistry(conventions);

			// todo optional element building security
			BaseElementBuilder.Security = new NoElementBuilderSecurity();
			SaveOrCancelConvention.Convention = new SaveOrCancelButtonsConvention();
			LabelingConvention.Convention = new SpaceBeforeCapitalsLabelingConvention();
			FiltersConventions.FilterButtonConvention = new FiltersFilterButtonConvention();
			FiltersConventions.ResetButtonConvention = new FiltersResetButtonConvention();
			PageActions.Convention = new PageActionConvention();
			PageActionConvention.Security = new PageActionsSecurity();
		}
	}
}