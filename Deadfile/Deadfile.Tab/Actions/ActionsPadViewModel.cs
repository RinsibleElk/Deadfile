using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Caliburn.Micro;
using Deadfile.Infrastructure.Interfaces;
using Deadfile.Tab.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using IEventAggregator = Prism.Events.IEventAggregator;

namespace Deadfile.Tab.Actions
{
    abstract class NavigationAwareActionsPadViewModel : PropertyChangedBase, INavigationAware
    {
        protected static readonly NLog.Logger Logger = NLog.LogManager.GetCurrentClassLogger();
        public abstract void OnNavigatedTo(object parameters);
        public abstract void OnNavigatedFrom();
    }

    abstract class ActionsPadViewModel<TPageState> : NavigationAwareActionsPadViewModel, IActionsPadViewModel
        where TPageState : struct
    {
        protected readonly TabIdentity TabIdentity;
        private SubscriptionToken _pageStateEventSubscriptionToken;
        protected IEventAggregator EventAggregator { get; private set; }

        public ActionsPadViewModel(TabIdentity tabIdentity,
            IEventAggregator eventAggregator)
        {
            TabIdentity = tabIdentity;
            EventAggregator = eventAggregator;
        }

        public void EditItem()
        {
            Logger.Info("Event|EditActionEvent|Send|{0}|{1}", TabIdentity.TabIndex, EditActionMessage.StartEditing);
            EventAggregator.GetEvent<EditActionEvent>().Publish(EditActionMessage.StartEditing);
        }

        public abstract bool CanSaveItem { get; }
        public abstract bool SaveItemIsVisible { get; }
        public abstract bool CanEditItem { get; }
        public abstract bool EditItemIsVisible { get; }

        public void SaveItem()
        {
            try
            {
                // Perform the save, and lock the item again.
                Logger.Info("Event|SaveEvent|Send|{0}|{1}", TabIdentity.TabIndex, SaveMessage.Save);
                EventAggregator.GetEvent<SaveEvent>().Publish(SaveMessage.Save);

                // Notify the other pages for the end of editing.
                Logger.Info("Event|EditActionEvent|Send|{0}|{1}", TabIdentity.TabIndex, EditActionMessage.EndEditing);
                EventAggregator.GetEvent<EditActionEvent>().Publish(EditActionMessage.EndEditing);
            }
            catch (Exception e)
            {
                Logger.Fatal(e, "Exception thrown during Save, {0}, {1}", e, e.StackTrace);
                throw;
            }
        }

        private TPageState _pageState = new TPageState();
        public TPageState PageState
        {
            get { return _pageState; }
            set
            {
                if (value.Equals(_pageState)) return;
                _pageState = value;
                NotifyOfPropertyChange(() => PageState);
                NotifyOfPropertyChange(() => CanSaveItem);
                NotifyOfPropertyChange(() => CanEditItem);
                NotifyOfPropertyChange(() => CanDeleteItem);
                NotifyOfPropertyChange(() => CanDiscardItem);
                NotifyOfPropertyChange(() => SaveItemIsVisible);
                NotifyOfPropertyChange(() => EditItemIsVisible);
                NotifyOfPropertyChange(() => DeleteItemIsVisible);
                NotifyOfPropertyChange(() => DiscardItemIsVisible);
                PageStateChanged(_pageState);
            }
        }

        protected abstract void PageStateChanged(TPageState state);

        private void PageStateAction(TPageState state)
        {
            Logger.Info("Event|PageStateEvent|Receive|{0}|{1}", TabIdentity.TabIndex, state);
            PageState = state;
        }

        public void DeleteItem()
        {
            // Perform the deletion.
            Logger.Info("Event|DeleteEvent|Send|{0}|{1}", TabIdentity.TabIndex, DeleteMessage.Delete);
            EventAggregator.GetEvent<DeleteEvent>().Publish(DeleteMessage.Delete);
        }

        public abstract bool CanDeleteItem { get; }

        public abstract bool DeleteItemIsVisible { get; }

        public void DiscardItem()
        {
            try
            {
                // Notify of the Discard. This leads to en masse Undo-ing.
                Logger.Info("Event|DiscardChangesEvent|Send|{0}|{1}", TabIdentity.TabIndex,
                    DiscardChangesMessage.Discard);
                EventAggregator.GetEvent<DiscardChangesEvent>().Publish(DiscardChangesMessage.Discard);

                // Notify the other pages for the end of editing.
                Logger.Info("Event|EditActionEvent|Send|{0}|{1}", TabIdentity.TabIndex, EditActionMessage.EndEditing);
                EventAggregator.GetEvent<EditActionEvent>().Publish(EditActionMessage.EndEditing);
            }
            catch (Exception e)
            {
                Logger.Fatal(e, "Exception thrown during Discard, {0}, {1}", e, e.StackTrace);
                throw;
            }
        }

        public abstract bool CanDiscardItem { get; }

        public abstract bool DiscardItemIsVisible { get; }

        public override void OnNavigatedTo(object parameters)
        {
            _pageStateEventSubscriptionToken =
                EventAggregator.GetEvent<PageStateEvent<TPageState>>().Subscribe(PageStateAction);
        }

        public override void OnNavigatedFrom()
        {
            // Unsubscribe for event from content.
            EventAggregator.GetEvent<PageStateEvent<TPageState>>().Unsubscribe(_pageStateEventSubscriptionToken);
            _pageStateEventSubscriptionToken = null;
        }
    }
}
