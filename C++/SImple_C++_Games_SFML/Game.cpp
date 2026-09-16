#include "Game.h"
using namespace sf;
using namespace std;

//Private Functions
void Game::initVariables()
{
	this->window = nullptr;

    //Game logic
    this->endgame = false;
    this->points = 0;
    this->enemySpawnTimerMax = 20.f;
    this->enemySpawnTimer = this->enemySpawnTimerMax;
    this->maxEnemies = 5;
    this->mouseheld = false;
    this->health = 20;
}
void Game::initWindow()
{
    this->videomode.height = 1080;
	this->videomode.width = 1920;
	this->window = new sf::RenderWindow(this->videomode, "Game 1", sf::Style::Titlebar | sf::Style::Close | sf::Style::Resize);
    this->window->setFramerateLimit(60);
   
}

void Game::initfonts()
{
    if (this->font.loadFromFile("Fonts/Spicy Potatos.ttf")) {
        cout<<"ERROR::GAME::INITFONTS::FAILED TO LOAD FONTS!"<< "\n";
    }
}
    
void Game::initEnemies()
{
    this->enemy.setSize(Vector2f(100.f, 100.f));
    this->enemy.setFillColor(Color::Cyan);
    this->enemy.setOutlineColor(Color::Green);
    this->enemy.setOutlineThickness(1.f);
    this->enemy.setPosition(10.f,10.f);
    this->enemy.setScale(0.5f, 0.5f);
}

void Game::inittext()
{
    this->uitext.setFont(this->font);
    this->uitext.setCharacterSize(12);
    this->uitext.setFillColor(Color::White);
    this->uitext.setString("NONE");
}

//Constructors and destructors
Game::Game()
{
	this->initVariables();
	this->initWindow();
    this->initEnemies();
    this->initfonts();
    this->inittext();
}
Game::~Game()
{
	delete this->window;
}
const bool Game::running() const
{
 	return this->window->isOpen();
}

const bool Game::getendgame() const
{
    return this->endgame;
}

//Functions

void Game::update()
{
    this->pollevent();
    if (this->endgame == false) {
        this->updateMouseposition();
        this->updatenemies();
    }
    //end game condition
    if (this->health <= 0) {
        this->endgame = true;
    }
}

void Game::render()
{
    /*
     -delete old frame
     -render objects
     -display frame in window

    renders the game objects
    */
    this->window->clear(Color::White);
    
    //draw game objects
    this->renderenemies(*this->window);
    this->rendertext(*this->window);
    this->window->display();
}

void Game::updateMouseposition()
{
    /*
    Updates mouse positons
    - Mouse position relative to window (vector 2i)
    */
    this->mousePosWindow = Mouse::getPosition(*this->window);
    this->mouseposview = this->window->mapPixelToCoords(this->mousePosWindow);
}

void Game::rendertext(RenderTarget& target)
{
}

void Game::updatetext()
{
    stringstream ss;
    ss << "Points :" << this->points<<"\n";
    ss << "Health:" << this->health << "\n";
    this->uitext.setString(ss.str());
}

void Game::spawnEnemy()
{
    /*
    spawns enemies and sets their positions randomly,type and colors.
    -sets a random positon.
    add enemy to the vector.
    */
    this->enemy.setPosition(
        static_cast<float>(rand() % static_cast<int> (this->window->getSize().x - this->enemy.getSize().x)),
        static_cast<float>(rand() % static_cast<int> (this->window->getSize().y - this->enemy.getSize().y))
    );

    //Randomize enemy type
    int type = rand() % 5;
    switch (type) {
    case 0:
        this->enemy.setFillColor(Color::Magenta);
        this->enemy.setSize(Vector2f(10.f, 10.f));
        break; 
    case 1:
        this->enemy.setFillColor(Color::Blue);
        this->enemy.setSize(Vector2f(30.f, 30.f));
        break;

    case 2:
        this->enemy.setFillColor(Color::Cyan);
        this->enemy.setSize(Vector2f(50.f, 50.f));
        break;
    case 3:
        this->enemy.setFillColor(Color::Red);
        this->enemy.setSize(Vector2f(70.f, 70.f));
        break;
    case 4:
        this->enemy.setFillColor(Color::Green);
        this->enemy.setSize(Vector2f(100.f, 100.f));
        break;

    default:
        this->enemy.setFillColor(Color::Yellow);
        this->enemy.setSize(Vector2f(200.f, 200.f));
        this->enemy.setSize(Vector2f(200.f, 200.f));
        break;
    }
    //spawns the enemy
    this->enemies.push_back(this->enemy);
}

void Game::updatenemies()
{
    /*
    updates the enemy spawn timer and spawns the enemies
    when the total amount of enemies is less than max enemies
    moves the enemies downwards.
    removes the  enemies at the edge of the screen

    */

    //updating the timer for enemy spawning
    if (this->enemies.size() < this->maxEnemies){
        if (this->enemySpawnTimer >= this->enemySpawnTimerMax) {
            //spawn the enemy and reset the timer
            this->spawnEnemy();
            this->enemySpawnTimer = 0.f;
        }
        else
            this->enemySpawnTimer += 1.f;
    }
   //Move and updating the enemies
    for (int i = 0; i < enemies.size(); i++) {
        bool deleted = false;
        this->enemies[i].move(0.f, 5.f);
        if (this->enemies[i].getPosition().y > this->window->getSize().y) {
            this->enemies.erase(this->enemies.begin() + i);
            this->health -= 1;
            cout << "Health: " << this->health << "\n";
        }
        //check if cliced upon
        if (Mouse::isButtonPressed(Mouse::Left)) {
            if (this->mouseheld == false) {
                this->mouseheld = true;
                bool deleted = false;
                for (size_t i = 0; i < this->enemies.size() && deleted == false; i++) {
                    if (this->enemies[i].getGlobalBounds().contains(this->mouseposview)) {
                        //points
                        if (this->enemies[i].getFillColor() == Color::Magenta)
                            this->points += 10;
                        else if (this->enemies[i].getFillColor() == Color::Blue)
                            this->points += 8;
                        else if (this->enemies[i].getFillColor() == Color::Cyan)
                            this->points += 7;
                        else if (this->enemies[i].getFillColor() == Color::Red)
                            this->points += 6;
                        else if (this->enemies[i].getFillColor() == Color::Green)
                            this->points += 5;
                        else if (this->enemies[i].getFillColor() == Color::Yellow)
                            this->points += 1;

                        cout << "Points: " << this->points << "\n";
                        deleted = true;
                        this->enemies.erase(this->enemies.begin() + i);
                        
                    }

                }
            }
            else {
                this->mouseheld = false;
            }

            //if the enemy has moved below the screen

            if (deleted) {
                this->enemies.erase(this->enemies.begin() + i);
            }
        }
    }

}
    void Game::pollevent() {
        //Event polling
        while (this->window->pollEvent(this->ev))
        {
            switch (this->ev.type)
            {
            case sf::Event::Closed:
                this->window->close();
                break;
            case sf::Event::KeyPressed:
                if (this->ev.key.code == sf::Keyboard::Escape)
                    this->window->close();
                break;
            }
        }
    }

void Game::renderenemies(RenderTarget& target)
{
    for (auto& e : this->enemies) {
        target.draw(e);
    }
}