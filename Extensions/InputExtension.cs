using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CoreUtility.Extensions {
    public static class InputExtension {
        #if ENABLE_INPUT_SYSTEM 
        public static bool TryAddScheme(this InputActionAsset asset, string key, params string[] requirements) {
            var isContainsScheme = asset.controlSchemes.FirstOrDefault((inputScheme) => key.Equals(inputScheme.bindingGroup));
            if (isContainsScheme != default)
                return false;

            var newControlScheme = new InputControlScheme(key);
            foreach (var requirementName in requirements) 
                newControlScheme.WithRequiredDevice(requirementName);
            
            asset.AddControlScheme(newControlScheme);

            return true;
        }

        public static bool TryAddBinding(this InputAction inputAction, string path, 
            string interactions = null, 
            string groups = null, 
            string processors = null) {

            if (inputAction.bindings.Any(b => b.path == path))
                return false;
            
            inputAction.AddBinding(path, interactions, processors, groups);
            return true;
        }
        
        public static bool TryAddAction(this InputActionMap map, string name, out InputAction action,
            InputActionType type = InputActionType.Value, 
            string binding = null, 
            string interactions = null, 
            string processors = null, 
            string groups = null, 
            string expectedControlLayout = null) {
            
            action = map.actions.FirstOrDefault(a => name.Equals(a.name));
            if (action != null)
                return false;

            action = map.AddAction(name, type, binding, interactions, processors, groups, expectedControlLayout);
            return true;
        }
        
        public static InputAction GetOrAdd(this InputActionMap map, string name,
            InputActionType type = InputActionType.Value, 
            string binding = null, 
            string interactions = null, 
            string processors = null, 
            string groups = null, 
            string expectedControlLayout = null) {
            
            return 
                map.actions.FirstOrDefault(bind => name.Equals(bind.name)) ?? 
                map.AddAction(name, type, binding, interactions, processors, groups, expectedControlLayout);
        }

        public static InputActionMap GetOrAdd(this InputActionAsset asset, string name) {
            return asset.actionMaps.FirstOrDefault(map => map.name.Equals(name)) ?? 
                   asset.AddActionMap(name);
        }

        
        /// <summary>
        /// Is null when find some bind with group, when didn't find return inputAction 
        /// </summary>
        public static InputAction IsGroups(this InputAction inputAction, string groups) {
            return inputAction.bindings.FirstOrDefault((bind) => 
                !string.IsNullOrEmpty(bind.groups) && 
                bind.groups.Equals(groups)) == default ? inputAction : null;
        }

#endif
    }
}