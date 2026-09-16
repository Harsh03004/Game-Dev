#pragma once
#include<iostream>
#include<vector>
#include<ctime>
#include<sstream>
#include <SFML/Graphics.hpp>
#include <SFML/Audio.hpp>
#include <SFML/Network.hpp>
#include <SFML/System.hpp>
#include <SFML/Window.hpp>
using namespace sf;
using namespace std;
/*
    CLass that acts as the game engine
	Wrapper Class
*/
class Game
{
private:
	//Variables
	// Window
	sf::RenderWindow* window;
	sf::Event ev;
	sf::VideoMode videomode;

	//resources
	sf::Font font;
	
	//Text
	sf::Text uitext;

	//Game logic 
	bool endgame;
	unsigned points;
	float enemySpawnTimer;
	float enemySpawnTimerMax;
	int maxEnemies;
	bool mouseheld;
	int health;
	int enemyspeed;

	//Mouse Positions
	Vector2i mousePosWindow;
	Vector2f mouseposview;

	//Game Objects
	RectangleShape enemy;
	vector<RectangleShape> enemies;

	//private functions
	void initfonts();
	void initVariables();
	void initWindow();
	void initEnemies();
	void inittext();	
public:
	//Constructors and destructors
	Game();
	virtual ~Game();

	//Accessors
	const bool running() const;
	const bool getendgame() const;

	// Functions
	void rendertext(RenderTarget& target);
	void updatetext();
	void spawnEnemy();
	void updatenemies();
	void renderenemies(RenderTarget& target);
	void pollevent();
	void update();
	void render();
	void updateMouseposition();
};

