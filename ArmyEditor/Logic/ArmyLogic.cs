using ArmyEditor.Models;
using ArmyEditor.Services;
using Microsoft.Toolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArmyEditor.Logic
{
    public class ArmyLogic : IArmyLogic
    {
        private IList<Trooper> barracks;
        private IList<Trooper> army;
        IMessenger messenger;
        ITrooperEditorService editorService;
        public ArmyLogic(IMessenger messenger, ITrooperEditorService editorService)
        {
            this.messenger = messenger; 
            this.editorService = editorService;
        }
        public void SetupCollections(IList<Trooper> barracks, IList<Trooper> army)
        {
            this.barracks = barracks;
            this.army = army;
        }
        public void AddToArmy(Trooper trooper)
        {
            army.Add(trooper);
            messenger.Send("Trooper added","TrooperInfo");
        }
        public void RemoveFromArmy(Trooper trooper)
        {
            army.Remove(trooper);
            messenger.Send("Trooper removed", "TrooperInfo");
        }
        public void EditTrooper(Trooper trooper)
        {
            editorService.Edit(trooper);
        }
        public int AllCost
        {
            get
            {
                return army.Count == 0 ? 0 : army.Sum(t => t.Cost);
            }
        }
        public double AvgPower
        {
            get
            {
                return army.Count == 0 ? 0 : army.Average(t => t.Power);
            }
        }
        public double AvgSpeed
        {
            get
            {
                return army.Count == 0 ? 0 : army.Average(t => t.Speed);
            }
        }
    }
}
