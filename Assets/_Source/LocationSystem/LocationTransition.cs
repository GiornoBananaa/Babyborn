using System;
using System.Collections.Generic;
using System.Linq;
using LocationSystem.UI;
using UnityEngine;

namespace LocationSystem
{
    public class LocationTransition
    {
        private Location _currentLocation;
        private Dictionary<LocationType,Location> _locations;
        private Camera _camera;
        public event Action<Location> OnLocationMoved;
        
        public LocationTransition(IEnumerable<TransitionButton> buttons, IEnumerable<Location> locations)
        {
            _locations = locations.ToDictionary(location => location.Type);
            _camera = Camera.main;
            foreach (var button in buttons)
            {
                button.Button.onClick.AddListener(( )=> MoveToLocation(_locations[button.Location]));
            }

            foreach (var location in _locations.Values)
            {
                location.MainPanel.gameObject.SetActive(false);
            }
            MoveToLocation(_locations[0]);
        }

        public void MoveToLocation(Location location)
        {
            if(_currentLocation == location) return;
            
            if(_currentLocation != null)
                _currentLocation.MainPanel.gameObject.SetActive(false);
            location.MainPanel.gameObject.SetActive(true);
            
            _camera.transform.position = location.CameraPosition.position;
            _currentLocation = location;
            OnLocationMoved?.Invoke(location);
        }
    }
}