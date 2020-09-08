using System;
using System.Diagnostics;
using System.Numerics;
using System.Windows.Media;
using Bot.Moves;
using Bot.Utilities.Processed.BallPrediction;
using Bot.Utilities.Processed.FieldInfo;
using Bot.Utilities.Processed.Packet;
using RLBotDotNet;

namespace Bot
{
    // We want to our bot to derive from Bot, and then implement its abstract methods.
    class Bot : RLBotDotNet.Bot
    {
        // We want the constructor for our Bot to extend from RLBotDotNet.Bot, but we don't want to add anything to it.
        // You might want to add logging initialisation or other types of setup up here before the bot starts.
        public Bot(string botName, int botTeam, int botIndex) : base(botName, botTeam, botIndex) { }

        
        int count = 0;
        bool toggle = true;
        Move move = new Move(BlueprintCreator.getHalfCabFlip());


        public override Controller GetOutput(rlbot.flat.GameTickPacket gameTickPacket)
        {
            // We process the gameTickPacket and convert it to our own internal data structure.
            Packet packet = new Packet(gameTickPacket);


            Vector3 ballLocation = packet.Ball.Physics.Location;
            Vector3 carLocation = packet.Players[index].Physics.Location;
            Orientation carRotation = packet.Players[index].Physics.Rotation;

            

            Vector3 ballRelativeLocation = Orientation.RelativeLocation(carLocation, ballLocation, carRotation);

            float steer;
            if (ballRelativeLocation.Y > 0)
                steer = 1;
            else
                steer = -1;
            
            // Examples of rendering in the game
            Renderer.DrawString3D("Ball", Colors.Black, ballLocation, 3, 3);
            Renderer.DrawString3D(steer > 0 ? "Right" : "Left", Colors.Aqua, carLocation, 3, 3);
            Renderer.DrawLine3D(Colors.Red, carLocation, ballLocation);



            count++;
            if (count > 120) {
                if (toggle) {
                    move = new Move(BlueprintCreator.waveDash());
                } else {
                    move = new Move(BlueprintCreator.waveDash());
                }

                move.start();

                toggle = !toggle;
                count = 0;
            }

            Input input = move.get();
            if (input.frame!=-1) {
                return new Controller {
                    Throttle = 0,
                    Pitch = input.pitch,
                    Roll = input.roll,
                    Jump = input.jump,
                    Yaw = input.yaw,
                    Steer = input.stear
                    
                };
            }

            float throttle = 1;
            if (toggle) {
                throttle = 1;
            }

            
            return new Controller {
                Throttle = throttle,

                Steer = 0

            };
        }
        
        // Hide the old methods that return Flatbuffers objects and use our own methods that
        // use processed versions of those objects instead.
        internal new FieldInfo GetFieldInfo() => new FieldInfo(base.GetFieldInfo());
        internal new BallPrediction GetBallPrediction() => new BallPrediction(base.GetBallPrediction());
    }
}