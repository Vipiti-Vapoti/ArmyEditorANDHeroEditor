using ArmyEditor.Logic;
using ArmyEditor.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace ArmyEditor.ViewModels
{
    public class MainWindowViewModel : ObservableRecipient
    {
        IArmyLogic logic;
        public ObservableCollection<Trooper> Barrack { get; set; }
        public ObservableCollection<Trooper> Army { get; set; }
        private Trooper selectedFromArmy;

        public Trooper SelectedFromArmy
        {
            get { return selectedFromArmy; }
            set 
            { 
                SetProperty(ref selectedFromArmy, value);  
                (RemoveFromArmy as RelayCommand).NotifyCanExecuteChanged();
            }
        }

        private Trooper selectedFromBarrack;

        public Trooper SelectedFromBarrack
        {
            get { return selectedFromBarrack; }
            set 
            {
                SetProperty(ref selectedFromBarrack, value);
                (AddToArmy as RelayCommand).NotifyCanExecuteChanged();
                (EditTrooper as RelayCommand).NotifyCanExecuteChanged();
            }
        }

        public int AllCost
        {
            get { return logic.AllCost; }
        }
        public double AVGPower { get { return logic.AvgPower; } }
        public double AVGSpeed { get { return logic.AvgSpeed; } }
        public static bool IsInDesignMode
        {
            get
            {
                var prop = DesignerProperties.IsInDesignModeProperty;
                return (bool)DependencyPropertyDescriptor.FromProperty(prop, typeof(FrameworkElement)).Metadata.DefaultValue;
            }
        }
        public  ICommand AddToArmy { get; set; }
        public ICommand RemoveFromArmy { get; set; }
        public ICommand EditTrooper { get; set; }
        public MainWindowViewModel()
            :this(IsInDesignMode? null : Ioc.Default.GetService<IArmyLogic>())//todo hidden dependecy resolve
        {

        }
        public MainWindowViewModel(IArmyLogic logic)
        {
            this.logic = logic;
            Barrack = new ObservableCollection<Trooper>();
            Army = new ObservableCollection<Trooper>();
            Barrack.Add(new Trooper() 
            { 
                Type="marine", 
                Power=8, 
                Speed=6
            });
            Barrack.Add(new Trooper()
            {
                Type = "pilot",
                Power = 10,
                Speed = 5
            });
            Barrack.Add(new Trooper()
            {
                Type = "submarine",
                Power = 9,
                Speed = 2
            });
            Barrack.Add(new Trooper()
            {
                Type = "infantry",
                Power = 9,
                Speed = 2
            });
            Barrack.Add(new Trooper()
            {
                Type = "sniper",
                Power = 9,
                Speed = 2
            });

            Army.Add(Barrack[2].GetCopy());
            Army.Add(Barrack[4].GetCopy());
            logic.SetupCollections(Barrack,Army);
            AddToArmy = new RelayCommand(
                () => logic.AddToArmy(SelectedFromBarrack),
                () => SelectedFromBarrack!=null
                );
            RemoveFromArmy = new RelayCommand(
                () =>logic.RemoveFromArmy(SelectedFromArmy),
                () => SelectedFromArmy != null
                );
            EditTrooper = new RelayCommand(
                () => logic.EditTrooper(SelectedFromBarrack),
                () => SelectedFromBarrack != null
                );
            Messenger.Register<MainWindowViewModel, string, string>(this, "TrooperInfo", (recipient, msg ) =>
            {
                OnPropertyChanged("AllCost");
                OnPropertyChanged("AVGPower");
                OnPropertyChanged("AVGSpeed");
            }
            );
        }
    }
}
