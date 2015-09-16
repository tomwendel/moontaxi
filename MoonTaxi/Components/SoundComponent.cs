using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using MoonTaxi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MoonTaxi.Components
{
    internal class SoundComponent : GameComponent
    {
        private SoundEffect engine;
        private SoundEffect scratch;
        private SoundEffect crash;
        private SoundEffect shout;

        private Dictionary<Taxi, SoundEffectInstance> engines = new Dictionary<Taxi, SoundEffectInstance>();
        private Dictionary<Taxi, SoundEffectInstance> scratches = new Dictionary<Taxi, SoundEffectInstance>();

        public SoundComponent(Game game) : base(game)
        {
        }

        public override void Initialize()
        {
            engine = Game.Content.Load<SoundEffect>("Sounds/triebwerk");
            scratch = Game.Content.Load<SoundEffect>("Sounds/schramm");
            crash = Game.Content.Load<SoundEffect>("Sounds/crash");
            shout = Game.Content.Load<SoundEffect>("Sounds/taxi");
        }

        public void AddTaxi(Taxi taxi)
        {
            SoundEffectInstance engineInstance = engine.CreateInstance();
            engineInstance.IsLooped = true;
            engineInstance.Volume = 0f;
            engineInstance.Play();

            engines.Add(taxi, engineInstance);

            SoundEffectInstance scratchInstance = scratch.CreateInstance();
            scratchInstance.IsLooped = true;
            scratchInstance.Volume = 0f;
            scratchInstance.Play();

            scratches.Add(taxi, scratchInstance);
        }

        public void RemoveTaxi(Taxi taxi)
        {
            SoundEffectInstance engineInstance = engines[taxi];
            engineInstance.Stop();
            engineInstance.Dispose();
            engines.Remove(taxi);

            SoundEffectInstance scratchInstance = scratches[taxi];
            scratchInstance.Stop();
            scratchInstance.Dispose();
            scratches.Remove(taxi);
        }

        public void PlayCrash(float volume)
        {
            crash.Play(volume, 0, 0);
        }

        public void PlayShout()
        {
            shout.Play(1, 0, 0);
        }

        internal void SetEngineVolume(Taxi taxi, float volume)
        {
            engines[taxi].Volume = volume;
        }

        internal void SetScratchVolume(Taxi taxi, float volume)
        {
            scratches[taxi].Volume = volume;
        }
        internal void SetScratchPitch(Taxi taxi, float pitch)
        {
            scratches[taxi].Pitch = pitch;
        }
    }
}
