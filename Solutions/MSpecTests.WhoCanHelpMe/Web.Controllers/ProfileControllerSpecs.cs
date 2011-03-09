//-------------------------------------------------------------------------------------------------
// <auto-generated> 
// Marked as auto-generated so StyleCop will ignore BDD style tests
// </auto-generated>
//-------------------------------------------------------------------------------------------------

#pragma warning disable 169
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local

namespace MSpecTests.WhoCanHelpMe.Web.Controllers
{
    #region Using Directives

    using System.Collections.Generic;
    using System.Web.Mvc;

    using global::WhoCanHelpMe.Domain;
    using global::WhoCanHelpMe.Domain.Contracts.Tasks;
    using SharpArch.Futures.Core.Mapping;
    using global::WhoCanHelpMe.Framework.Security;
    using global::WhoCanHelpMe.Web.Controllers.Home;
    using global::WhoCanHelpMe.Web.Controllers.Profile;
    using global::WhoCanHelpMe.Web.Controllers.Profile.Mappers.Contracts;
    using global::WhoCanHelpMe.Web.Controllers.Profile.ViewModels;
    using global::WhoCanHelpMe.Web.Controllers.Shared.ActionResults;
    using Machine.Specifications;
    using Machine.Specifications.AutoMocking.Rhino;
    using Rhino.Mocks;

    using xVal.ServerSide;
    using Machine.Specifications.Mvc;

    #endregion;

    public abstract class specification_for_profile_controller : Specification<ProfileController>
    {
        protected static IProfileTasks user_tasks;
        protected static IIdentityService identity_service;
        protected static ICategoryTasks category_tasks;
        protected static IMapper<Profile, ViewProfilePageViewModel> view_profile_view_model_mapper;
        protected static IMapper<Profile, IList<Category>, UpdateProfilePageViewModel> update_profile_view_model_mapper;
        protected static ICreateProfilePageViewModelBuilder create_profile_page_view_model_builder;
        protected static ITagTasks tag_tasks;
            
        Establish context = () =>
            {
                identity_service = DependencyOf<IIdentityService>();
                user_tasks = DependencyOf<IProfileTasks>();
                category_tasks = DependencyOf<ICategoryTasks>();
                create_profile_page_view_model_builder = DependencyOf<ICreateProfilePageViewModelBuilder>();
                view_profile_view_model_mapper = DependencyOf<IMapper<Profile, ViewProfilePageViewModel>>();
                update_profile_view_model_mapper = DependencyOf<IMapper<Profile, IList<Category>, UpdateProfilePageViewModel>>();
            };
    }

    [Subject(typeof(ProfileController))]
    public class when_the_profile_controller_is_asked_for_the_create_view : specification_for_profile_controller
    {
        static ActionResult result;
        static CreateProfilePageViewModel the_view_model;

        Establish context = () =>
            {
                the_view_model = new CreateProfilePageViewModel();

                create_profile_page_view_model_builder.Stub(m => m.Get()).Return(the_view_model);
            };

        Because of = () => result = subject.Create();

        It should_ask_the_create_profile_page_view_model_builder_to_map_the_view_model =
            () => create_profile_page_view_model_builder.AssertWasCalled(m => m.Get());

        It should_return_the_default_view =
            () => result.ShouldBeAView().And().ShouldUseDefaultView();

        It should_pass_the_view_model_to_the_view =
            () => result.Model<CreateProfilePageViewModel>().ShouldBeTheSameAs(the_view_model);

    }

    [Subject(typeof(ProfileController))]
    public class when_the_profile_controller_is_told_to_create_a_new_profile : specification_for_profile_controller
    {
        static string user_name;
        static Identity the_identity;
        static ActionResult result;
        static CreateProfileFormModel create_profile_view_model;

        Establish context = () =>
            {
                user_name = "user_name";

                the_identity = new Identity(user_name, string.Empty, true);

                create_profile_view_model = new CreateProfileFormModel
                    {
                        FirstName = "First name", 
                        LastName = "Last name"
                    };

                identity_service.Stub(i => i.GetCurrentIdentity()).Return(the_identity);
            };

        Because of = () => result = subject.Create(create_profile_view_model);

        It should_ask_the_identity_service_for_the_logged_in_user =
            () => identity_service.AssertWasCalled(i => i.GetCurrentIdentity());

        It should_ask_the_user_tasks_to_create_the_new_profile =
            () => user_tasks.AssertWasCalled(u => u.CreateProfile(the_identity.Name, create_profile_view_model.FirstName, create_profile_view_model.LastName));

        It should_redirect_to_the_update_action =
            () => result.ShouldRedirectToAction<ProfileController>(x => x.Update()); 
    }

    [Subject(typeof(ProfileController))]
    public class when_the_profile_controller_is_asked_to_delete_a_profile : specification_for_profile_controller
    {
        static Identity the_identity;
        static ActionResult result;
        static string user_name;

        Establish context = () =>
            {
                user_name = "User name";
                the_identity = new Identity(user_name, string.Empty, true);

                identity_service.Stub(i => i.GetCurrentIdentity()).Return(the_identity);
            };

        Because b = () => result = subject.Delete();

        It should_ask_the_identity_service_for_the_logged_in_user =
            () => identity_service.AssertWasCalled(i => i.GetCurrentIdentity());

        It should_ask_the_user_tasks_to_delete_the_profile =
            () => user_tasks.AssertWasCalled(u => u.DeleteProfile(user_name));

        It should_redirect_to_home =
            () => result.ShouldRedirectToAction<HomeController>(x => x.Index());
    }

    [Subject(typeof(ProfileController))]
    public class when_the_profile_controller_is_asked_to_delete_an_assertion : specification_for_profile_controller
    {
        static ActionResult result;
        static Profile the_retrieved_profile;
        static int the_assertion_id;
        static string the_user_name;
        static Identity the_current_identity;

        Establish context = () =>
        {
            the_user_name = "domain\account";

            the_current_identity = new Identity(the_user_name, string.Empty, true);

            the_retrieved_profile = new Profile();

            the_assertion_id = 100;

            identity_service.Stub(it => it.GetCurrentIdentity()).Return(the_current_identity);

            user_tasks.Stub(ut => ut.GetProfileByUserName(the_user_name)).Return(the_retrieved_profile);
        };

        Because of = () => result = subject.DeleteAssertion(the_assertion_id);

        It should_ask_the_identity_service_for_the_current_identity =
            () => identity_service.AssertWasCalled(i => i.GetCurrentIdentity());

        It should_ask_the_user_tasks_for_the_user =
            () => user_tasks.AssertWasCalled(u => u.GetProfileByUserName(the_user_name));

        It should_tell_the_user_tasks_to_remove_the_assertion =
            () => user_tasks.AssertWasCalled(u => u.RemoveAssertion(the_retrieved_profile, the_assertion_id));

        It should_redirect_to_the_update_action =
            () => result.ShouldRedirectToAction<ProfileController>(x => x.Update());
    }

    [Subject(typeof(ProfileController))]
    public class when_the_profile_controller_is_asked_for_the_default_action : specification_for_profile_controller
    {
        static ActionResult result;

        Because of = () => result = subject.Index();

        It should_redirect_to_the_update_action = () => result.ShouldRedirectToAction<ProfileController>(x => x.Update());
    }

    [Subject(typeof(ProfileController))]
    public class when_the_profile_controller_is_asked_for_the_update_view : specification_for_profile_controller
    {
        static string the_user_name;
        static Identity the_current_identity;
        static Profile the_retrieved_profile;
        static List<Category> all_categories;
        static ActionResult result;
        static UpdateProfilePageViewModel the_view_model;

        Establish context = () =>
        {
            the_user_name = "domain\account";

            the_current_identity = new Identity(the_user_name, string.Empty, true);

            the_retrieved_profile = new Profile();

            the_view_model = new UpdateProfilePageViewModel();

            user_tasks.Stub(ut => ut.GetProfileByUserName(the_user_name)).Return(the_retrieved_profile);

            all_categories = new List<Category>();

            category_tasks.Stub(ct => ct.GetAll()).Return(all_categories);

            identity_service.Stub(it => it.GetCurrentIdentity()).Return(the_current_identity);

            update_profile_view_model_mapper.Stub(pvmm => pvmm.MapFrom(the_retrieved_profile, all_categories)).Return(the_view_model);
        };

        Because of = () => result = subject.Update();

        It should_ask_the_identity_service_for_the_current_identity =
            () => identity_service.AssertWasCalled(i => i.GetCurrentIdentity());

        It should_ask_the_user_tasks_for_the_user =
            () => user_tasks.AssertWasCalled(u => u.GetProfileByUserName(the_user_name));

        It should_ask_the_category_tasks_for_all_categories =
            () => category_tasks.AssertWasCalled(c => c.GetAll());

        It should_ask_the_profile_view_model_mapper_to_map_the_user_and_categories =
            () => update_profile_view_model_mapper.AssertWasCalled(m => m.MapFrom(the_retrieved_profile, all_categories));

        It should_return_the_profile_view = () =>
            result.ShouldBeAView().And().ShouldUseDefaultView();

        It should_pass_the_view_model_to_the_profile_view =
            () => result.Model<UpdateProfilePageViewModel>().ShouldBeTheSameAs(the_view_model);
    }

    [Subject(typeof(ProfileController))]
    public class when_the_profile_controller_is_told_to_update_a_profile : specification_for_profile_controller
    {
        static ActionResult result;
        static AddAssertionFormModel the_view_model;
        static Identity the_identity;

        Establish context = () =>
            {
                the_view_model = new AddAssertionFormModel
                    {
                        CategoryId = 1,
                        TagName = "Tag name"
                    };

                the_identity = new Identity("User name", string.Empty, true);

                identity_service.Stub(i => i.GetCurrentIdentity()).Return(the_identity);
            };

        Because of = () => result = subject.Update(the_view_model);

        It should_ask_the_identity_service_for_the_current_identity =
            () => identity_service.AssertWasCalled(i => i.GetCurrentIdentity());

        It should_ask_the_user_tasks_to_add_the_new_assertion =
            () => user_tasks.AssertWasCalled(u => u.AddAssertion(the_identity.Name, the_view_model.CategoryId, the_view_model.TagName));

        It should_redirect_to_the_update_action =
            () => result.ShouldRedirectToAction<ProfileController>(x => x.Update());
    }

    [Subject(typeof(ProfileController))]
    public class when_the_profile_controller_is_asked_for_a_specific_profile : specification_for_profile_controller
    {
        static int the_user_id;
        static Profile the_retrieved_profile;
        static ActionResult result;
        static ViewProfilePageViewModel the_view_model;

        Establish context = () =>
        {
            the_user_id = 10;

            the_retrieved_profile = new Profile();

            user_tasks.Stub(ut => ut.GetProfileById(the_user_id)).Return(the_retrieved_profile);

            the_view_model = new ViewProfilePageViewModel();

            view_profile_view_model_mapper.Stub(pvmm => pvmm.MapFrom(the_retrieved_profile)).Return(the_view_model);
        };

        Because of = () => result = subject.View(the_user_id);

        It should_ask_the_user_tasks_for_the_user =
            () => user_tasks.AssertWasCalled(u => u.GetProfileById(the_user_id));

        It should_ask_the_profile_view_model_mapper_to_map_the_user =
            () => view_profile_view_model_mapper.AssertWasCalled(m => m.MapFrom(the_retrieved_profile));

        It should_return_the_default_view = () =>
            result.ShouldBeAView().And().ShouldUseDefaultView();

        It should_pass_the_view_model_to_the_profile_view =
            () => result.Model<ViewProfilePageViewModel>().ShouldBeTheSameAs(the_view_model);
    }

    [Subject(typeof(ProfileController))]
    public class when_the_profile_controller_is_asked_for_a_specific_profile_but_the_profile_does_not_exist : specification_for_profile_controller
    {
        static ActionResult result;
        static int the_user_id;

        Establish context = () =>
            {
                the_user_id = 10;

                user_tasks.Stub(ut => ut.GetProfileById(the_user_id)).Return(null);
            };

        Because of = () => result = subject.View(the_user_id);

        It should_ask_the_user_tasks_for_the_profile = () => user_tasks.AssertWasCalled(u => u.GetProfileById(the_user_id));

        It should_return_a_not_found_result = () => result.ShouldBeOfType<NotFoundResult>();
    }
}
