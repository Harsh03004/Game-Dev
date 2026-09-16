#include <iostream>
#include "Game.h"

using namespace sf;
using namespace std;

int main()
{
    //init srand
    srand(static_cast<unsigned>(time(NULL)));
  
    // init game engine
    Game game;

    // Game Loop
    while (game.running()&& !game.getendgame())
    {
        // Event polling
 
            // update
            game.update();
            // render
            game.render();
    }

    // End of app
    return 0;
}
