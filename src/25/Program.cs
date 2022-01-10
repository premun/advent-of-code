using Common;

char[][] seafloor = Resources.GetResourceFileLines("input.txt").ParseAsJaggedArray();

int width = seafloor[0].Length;
int height = seafloor.Length;

static void Display(char[][] seafloor)
{
    Console.WriteLine(new string(seafloor.SelectMany(row => row.Select(c => c).Append('\r').Append('\n')).ToArray()));
}

bool cucumberMoved;
int step = 0;

// Legend
// .    Empty field
// v    Cucumber going south
// V    Cucumber that just went south this round
// >    Cucumber going east
// @    Right-most cucumber blocked by left-most cucumber
// _    Empty spot after a cucumber that went from bottom to top
do
{
    step++;
    cucumberMoved = false;

    for (int y = 0; y < height; y++)
    {
        var prevY = (y + height - 1) % height;

        for (int x = 0; x < width; x++)
        {
            var curX = x;

            // Check if we can move out from the current field to right
            if (seafloor[y][curX] == '>')
            {
                var nextX = (curX + 1) % width;

                // > moves forward
                if (seafloor[y][nextX] == '.')
                {
                    cucumberMoved = true;
                    seafloor[y][nextX] = '>';
                    seafloor[y][curX] = '.';

                    if (curX == 0 && seafloor[y][width - 1] == '>')
                    {
                        // Make the right-most `>` not move this round
                        // As it is blocked by us
                        seafloor[y][width - 1] = '@';
                    }

                    // We can skip next field as it's blocked
                    x++;
                }
                else
                {
                    // We have >> situation (being the left >)
                    continue;
                }
            }

            // Are we free now and are we not gonna get blocked by right-most '>'?
            if (seafloor[y][curX] == '.' && (curX != 0 || seafloor[y][width - 1] != '>') && seafloor[prevY][curX] == 'v')
            {
                cucumberMoved = true;
                seafloor[prevY][curX] = y == 0 ? '_' : '.';

                // We mark the v with V so that we don't move it again in current step
                seafloor[y][curX] = y == height - 1 ? 'v' : 'V';
            }
            else if (seafloor[y][curX] == 'V') // Capital V has moved the last round and we have to correct it 
            {
                seafloor[y][curX] = 'v';
            }

            // This field is right column and represents a blocked `>`
            if (seafloor[y][curX] == '@')
            {
                seafloor[y][curX] = '>';
            }

            // _ appears in the bottom row and means there is v moving from there this round
            if (seafloor[y][curX] == '_')
            {
                seafloor[y][curX] = '.';
            }
        }
    }
} while (cucumberMoved);

Console.WriteLine($"Part 1: {step}");
Display(seafloor);
