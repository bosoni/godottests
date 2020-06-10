extends Node

const VERSION := "0.1.200609"; # year month day

var env; # WorldEnvironment node
var gameRunning := false;

var Module = load("res://scripts/Module.gd").new();

var mouseClicked := false;

