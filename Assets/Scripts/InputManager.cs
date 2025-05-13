using System;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour 
{
    public static InputManager Instance;
    
    // Public fields for inspector access
    public KeyCode MoveUp = KeyCode.W;
    public KeyCode MoveDown = KeyCode.S;
    public KeyCode MoveLeft = KeyCode.A;
    public KeyCode MoveRight = KeyCode.D;
    public KeyCode Dash = KeyCode.Space;
    public KeyCode CameraLeft = KeyCode.Q;
    public KeyCode CameraRight = KeyCode.E;
    public KeyCode Pause = KeyCode.Escape;
    public KeyCode AutoFire = KeyCode.R;
    
    private Dictionary<string, KeyCode> keyBindings = new Dictionary<string, KeyCode>();
    private Dictionary<string, KeyCode> defaultBindings = new Dictionary<string, KeyCode>();

    public bool isRebinding = false;

    private void Awake() 
    {
            Instance = this;
            InitializeDefaultBindings();
            InitializeKeybinds();
            LoadBindings();
    }
    
    private void InitializeDefaultBindings()
    {
        defaultBindings.Add("MoveUp", KeyCode.W);
        defaultBindings.Add("MoveDown", KeyCode.S);
        defaultBindings.Add("MoveLeft", KeyCode.A);
        defaultBindings.Add("MoveRight", KeyCode.D);
        defaultBindings.Add("Dash", KeyCode.Space);
        defaultBindings.Add("CameraLeft", KeyCode.Q);
        defaultBindings.Add("CameraRight", KeyCode.E);
        defaultBindings.Add("Pause", KeyCode.Escape);
        defaultBindings.Add("AutoFire", KeyCode.R);
    }

    private void InitializeKeybinds() 
    {
        foreach (var binding in defaultBindings)
        {
            keyBindings.Add(binding.Key, binding.Value);
        }
    }
    
    public void SetKey(string action, KeyCode newKey) 
    {
        if (!keyBindings.ContainsKey(action))
        {
            Debug.LogError($"Unknown action: {action}");
            return;
        }

        // Check for key conflicts
        foreach (var binding in keyBindings) 
        {
            if (binding.Value == newKey && binding.Key != action) 
            {
                Debug.LogWarning($"Key {newKey} is already assigned to {binding.Key}");
                return;
            }
        }
        
        keyBindings[action] = newKey;
        
        switch (action) 
        {
            case "MoveUp": MoveUp = newKey; break;
            case "MoveDown": MoveDown = newKey; break;
            case "MoveLeft": MoveLeft = newKey; break;
            case "MoveRight": MoveRight = newKey; break;
            case "Dash": Dash = newKey; break;
            case "CameraLeft": CameraLeft = newKey; break;
            case "CameraRight": CameraRight = newKey; break;
            case "Pause": Pause = newKey; break;
            case "AutoFire": AutoFire = newKey; break;
        }
        
        SaveBindings();
    }

    public void ResetToDefaults()
    {
        foreach (var binding in defaultBindings)
        {
            SetKey(binding.Key, binding.Value);
        }
    }

    public void SaveBindings() 
    {
        foreach (var binding in keyBindings) 
        {
            PlayerPrefs.SetString(binding.Key, binding.Value.ToString());
        }
        PlayerPrefs.Save();
    }

    public void LoadBindings() 
    {
        // Create a temporary list of keys to avoid modifying during iteration
        List<string> keys = new List<string>(keyBindings.Keys);
        
        foreach(var action in keys) 
        {
            if (PlayerPrefs.HasKey(action)) 
            {
                try 
                {
                    string savedValue = PlayerPrefs.GetString(action);
                    
                    if (Enum.TryParse<KeyCode>(savedValue, out KeyCode savedKey))
                    {
                        if (savedKey != KeyCode.None)
                        {
                            keyBindings[action] = savedKey;
                            
                            switch (action) 
                            {
                                case "MoveUp": MoveUp = savedKey; break;
                                case "MoveDown": MoveDown = savedKey; break;
                                case "MoveLeft": MoveLeft = savedKey; break;
                                case "MoveRight": MoveRight = savedKey; break;
                                case "Dash": Dash = savedKey; break;
                                case "CameraLeft": CameraLeft = savedKey; break;
                                case "CameraRight": CameraRight = savedKey; break;
                                case "Pause": Pause = savedKey; break;
                                case "AutoFire": AutoFire = savedKey; break;
                            }
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"Failed to parse key binding for {action}: {savedValue}");
                    }
                }
                catch (Exception e) 
                {
                    Debug.LogError($"Error loading binding for {action}: {e.Message}");
                    ResetToDefault(action);
                }
            }
        }
    }

    private void ResetToDefault(string action)
    {
        if (defaultBindings.ContainsKey(action))
        {
            SetKey(action, defaultBindings[action]);
        }
    }
    
    public bool GetKey(string action)
    {
        return keyBindings.ContainsKey(action) && Input.GetKey(keyBindings[action]);
    }
    
    public bool GetKeyDown(string action)
    {
        return keyBindings.ContainsKey(action) && Input.GetKeyDown(keyBindings[action]);
    }
    
    public bool GetKeyUp(string action)
    {
        return keyBindings.ContainsKey(action) && Input.GetKeyUp(keyBindings[action]);
    }
}