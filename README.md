# EARIN EX6
The goal of this project was to create a program distinguishing between normal and cancerous patterns of mass-spectrometric data.  
Created in C# using ML.NET machine learning library by Oskar Hącel & Marcin Lisowski  

Politechnika Warszawska, 05.2021

## Preparation
1. Download release package [here](https://github.com/KlivenPL/EARIN_EX6/releases) or build project yourself
1. Release package supports Windows machines
## Build
Build using newest version of Visual Studio 2019 (16.9.0) and .Net Core 3.1
## Data preparation
Given [dataset](https://archive.ics.uci.edu/ml/datasets/Arcene) consisted of following files (only files used in this project are enumerated here):  
*	```arcene_train.data``` – which contained training data for neural network
*	```arcene_train.labels``` – which contained labels (results) for arcene_train data
*	```arcene_valid.data``` – which contained test data for neural network. 
*	```arcene_valid.labels``` – which contained labels (results) for arcene_valid data
We merged .data datasets with .labels results in a way, that first column of newly created set contained the label (result) while the rest contained mass-spectrometric data.   Finally we obtained:
*	```arcene_train_labeled.txt``` – complete train set for neural network
*	```arcene_valid_labeled.txt``` – complete validation set for neural network.
Both described above sets consist of 10001 columns (result + mass-spectrometric data) and 100 rows (particular spectrograms)

## Results and more info
Check [PDF report](https://github.com/KlivenPL/EARIN_EX6/releases/download/1.0/EARIN_EX6_Oskar_Hacel_Marcin_Lisowski.pdf) and [release](https://github.com/KlivenPL/EARIN_EX6/releases/tag/1.0) for results and more information
