using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace HololiveFightingGame.Loading.Serializable
{
    public class ControlProfileLoader
    {
        public string Attack { get; set; }

        public string AttackB { get; set; }

        public string ControlLeft { get; set; }

        public string ControlRight { get; set; }

        public string ControlUp { get; set; }

        public string ControlDown { get; set; }

        public string Jump { get; set; }
    }
}
