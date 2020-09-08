using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Bot.Utilities.Processed.BallPrediction;
using Bot.Utilities.Processed.FieldInfo;
using Bot.Utilities.Processed.Packet;
using rlbot.flat;
using RLBotDotNet;


namespace Bot.Moves {


    public class Move {
        int count = 0;
        Inputs inputs = new Inputs();
        InputBlueprint blueprint;
        public Move(InputBlueprint inputBlueprint) {
            blueprint = inputBlueprint;
        }



        public Input get() {
            count++;
            return inputs.getInput(count - 1, blueprint);
        }
        public void start() {
            inputs = new Inputs();
            count = 0;
        }
    }

    class Inputs{

        public Input getInput(int frame, InputBlueprint inputBlueprint) {

            bool jump = false;
            Input input = new Input(0, 0, 0, 0, 0);

            if (frame > inputBlueprint.length) {
                input.frame = -1;
                return input;
            } else {
                Console.WriteLine(frame);
            }

            Debug.WriteLine("----- "+inputBlueprint.jumptoggleFrames);

            foreach (int jumpToggleFrame in inputBlueprint.jumptoggleFrames) {
                if (jumpToggleFrame <= frame) {
                    jump = !jump;
                }
            }

            foreach (Input controllerFrame in inputBlueprint.controllerFrames) {
                if (controllerFrame.frame <= frame) {
                    input = controllerFrame;
                }
            }
            input.jump = jump;
            return input;
        }


    }


    public struct Input {
        public int frame;
        public float pitch;
        public float stear;
        public float roll;
        public bool jump;
        public float yaw;

        public Input(int frame, float pitch, float stear, float roll,float yaw) {
            this.frame = frame;
            this.pitch = pitch;
            this.stear = stear;
            this.roll = roll;
            this.yaw = yaw;
            jump = false;

        }

    }



    public struct InputBlueprint {

        public int length;
        public List<int> jumptoggleFrames;
        public List<Input> controllerFrames;

        public InputBlueprint(int length) {
            this.length = length;
            jumptoggleFrames = new List<int>();
            controllerFrames = new List<Input>();
        }

    }


    public static class BlueprintCreator {
        public static InputBlueprint getHalfCabFlip() {
            int blueprintLength = 78;
            InputBlueprint blueprint = new InputBlueprint(blueprintLength);

            blueprint.jumptoggleFrames.Add(0);
            blueprint.controllerFrames.Add(new Input(0, 0, 0, 0, 1));
            blueprint.jumptoggleFrames.Add(3);

            blueprint.controllerFrames.Add(new Input(5, 1, 0, 0, 0));
            blueprint.jumptoggleFrames.Add(5);
            blueprint.jumptoggleFrames.Add(6);
            blueprint.controllerFrames.Add(new Input(20, -1, 0, 0, -1));
            blueprint.controllerFrames.Add(new Input(30, -1, 0, 1, 0));
            blueprint.controllerFrames.Add(new Input(40, -1, 0, 1, 0));
            blueprint.controllerFrames.Add(new Input(60, -1, 0, 0, 1));

            return blueprint;

        }

        public static InputBlueprint getHalfCabFlip2() {

            int blueprintLength = 83;
            InputBlueprint blueprint = new InputBlueprint(blueprintLength);

            blueprint.jumptoggleFrames.Add(0);
            blueprint.controllerFrames.Add(new Input(0, 0, 0, 0, 0));
            blueprint.jumptoggleFrames.Add(3);

            blueprint.controllerFrames.Add(new Input(10, -1, 0, 0, 0));
            blueprint.jumptoggleFrames.Add(10);
            blueprint.jumptoggleFrames.Add(11);
            blueprint.controllerFrames.Add(new Input(25, 1, 0, 0, 1));
            blueprint.controllerFrames.Add(new Input(35, 1, 0, 1, 0));
            blueprint.controllerFrames.Add(new Input(65, 1, 0, 0, 0));

            return blueprint;


        }

        public static InputBlueprint stall() {
            int blueprintLength = 1;
            InputBlueprint blueprint = new InputBlueprint(blueprintLength);
            
            blueprint.controllerFrames.Add(new Input(0, 0, 0, -1, 1));
            blueprint.jumptoggleFrames.Add(0);
            blueprint.controllerFrames.Add(new Input(1, 0, 0, 0, 0));
            blueprint.jumptoggleFrames.Add(1);

            return blueprint;


        }


        public static InputBlueprint waveDash() {
            int blueprintLength = 50;
            InputBlueprint blueprint = new InputBlueprint(blueprintLength);

            blueprint.jumptoggleFrames.Add(0);
            blueprint.jumptoggleFrames.Add(1);

            blueprint.controllerFrames.Add(new Input(32, 0, 0, -1, 0));
            blueprint.controllerFrames.Add(new Input(45, 0, 0, 0, 0));
            blueprint.controllerFrames.Add(new Input(49, 0, 0, 1, 0));
            blueprint.jumptoggleFrames.Add(49);

            blueprint.controllerFrames.Add(new Input(50, 0, 0, 0, 0));
            blueprint.jumptoggleFrames.Add(50);


            return blueprint;


        }


    }


}
