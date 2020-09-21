using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace War_Card_Game
{
    public class Cards
    {
        const int NUM_CARDS = 52;
        const int NUM_WAR_CARDS = 3;
        const int CARD_NUM_OFFSET = 2;  // lists are 0-based but playing cards start at 2
        const string PLAYER_1 = "Player 1";
        const string PLAYER_2 = "Player 2";
        enum Suits
        {
            CLUBS,
            HEARTS,
            SPADES,
            DIAMONDS,
            NUM_SUITS
        }

        const int NUM_CARDS_IN_SUIT = NUM_CARDS / (int)Suits.NUM_SUITS;

        enum HighCards
        {
            JACK = 9,
            QUEEN,
            KING,
            ACE
        }

        private readonly bool manualPlay;
        private bool isWar;
        private List<int> player1Deck;
        private List<int> player2Deck;
        private List<int> cardsToAdd;

        /// <summary>
        /// Card constructor. Creates the deck, shuffles the cards, and then distributes half to each player.
        /// </summary>
        /// <param name="manual"></param>
        public Cards(bool manual)
        {
            manualPlay = manual;
            isWar = false;
            cardsToAdd = new List<int>();

            var cards = new List<int>();
            for (int i = 0; i < NUM_CARDS; i++)
            {
                cards.Add(i);
            }

            var rand = new Random();
            var shuffledCards = cards.OrderBy(x => rand.Next()).ToList();

            player1Deck = shuffledCards.Take(shuffledCards.Count / 2).ToList();
            player2Deck = shuffledCards.Skip(shuffledCards.Count / 2).ToList();

            /*Console.WriteLine("deck 1:");
            for (int i = 0; i < player1Deck.Count; i++)
            {
                Console.Write(player1Deck[i] + ", ");
            }
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("deck 2:");
            for (int i = 0; i < player2Deck.Count; i++)
            {
                Console.Write(player2Deck[i] + ", ");
            }*/
            //Console.WriteLine(Environment.NewLine + "Press any key to begin");

            //Console.ReadKey();
            Console.WriteLine("");
        }

        /// <summary>
        /// Main Game Loop
        /// </summary>
        public void Play()
        {
            while(player1Deck.Count > 0 && player2Deck.Count > 0)
            {
                CompareCards();
                /*Console.WriteLine("deck 1:");
                for (int i = 0; i < player1Deck.Count; i++)
                {
                    Console.Write(player1Deck[i] + ", ");
                }
                Console.WriteLine("");
                Console.WriteLine("deck 2:");
                for (int i = 0; i < player2Deck.Count; i++)
                {
                    Console.Write(player2Deck[i] + ", ");
                }
                Console.WriteLine("");*/
                if (manualPlay)
                {
                    Console.ReadKey();
                }
            }
            if (player2Deck.Count == 0)
            {
                Console.WriteLine(PLAYER_1 + " Wins!" + Environment.NewLine);
            }
            else if (player1Deck.Count == 0)
            {
                Console.WriteLine(PLAYER_2 + " Wins!" + Environment.NewLine);
            }
            Console.ReadKey();
        }

        /// <summary>
        ///Compare the top card of each player and determine the winner, or if they are equal it's WAR  
        /// </summary>
        private void CompareCards()
        {
            int player1Card = player1Deck[0];
            int player2Card = player2Deck[0];

            cardsToAdd.Add(player1Card);
            cardsToAdd.Add(player2Card);

            player1Deck.RemoveAt(0);
            player2Deck.RemoveAt(0);

            int player1CardVal = player1Card % NUM_CARDS_IN_SUIT;
            int player2CardVal = player2Card % NUM_CARDS_IN_SUIT;

            if (manualPlay)
            {
                //Console.WriteLine(player1Card);
                int player1Suit = player1Card / NUM_CARDS_IN_SUIT;
                PrintCard(PLAYER_1, player1CardVal, player1Suit);

                //Console.WriteLine(player2Card);
                int player2Suit = player2Card / NUM_CARDS_IN_SUIT;
                PrintCard(PLAYER_2, player2CardVal, player2Suit);
            }

            if (player1CardVal > player2CardVal)
            {
                // player 1 wins round
                PrintRoundWinner(PLAYER_1);
                EndRound(ref player1Deck);
            }
            else if (player1CardVal < player2CardVal)
            {
                // player 2 wins round
                PrintRoundWinner(PLAYER_2);
                EndRound(ref player2Deck);
            }
            // they are equal, so it's WAR!
            else
            {
                ResolveWar();
            }
        }

        /// <summary>
        /// End the round and add the accumulated cards to the winner's deck
        /// </summary>
        /// <param name="deck">the deck to receive the cards</param>
        private void EndRound(ref List<int> deck)
        {
            isWar = false;
            foreach (var card in cardsToAdd)
            {
                deck.Add(card);
            }

            cardsToAdd.Clear();
        }

        /// <summary>
        /// Remove the War Cards from the decks and compare the next card.
        /// If either player doesn't have enough cards for War, GAME OVER
        /// </summary>
        private void ResolveWar()
        {
            Console.WriteLine(Environment.NewLine + "War!" + Environment.NewLine);
            Console.ReadKey();
            isWar = true;
            if (player1Deck.Count < NUM_WAR_CARDS + 1)
            {
                Console.WriteLine("Player 1 does not have enough cards for war.");
                player1Deck.Clear();
                return;
            }
            else if (player2Deck.Count < NUM_WAR_CARDS + 1)
            {
                Console.WriteLine("Player 2 does not have enough cards for war.");
                player2Deck.Clear();
                return;
            }
            for (int i = 0; i < NUM_WAR_CARDS; i++)
            {
                cardsToAdd.Add(player1Deck[i]);
                cardsToAdd.Add(player2Deck[i]);
            }
            player1Deck.RemoveRange(0, NUM_WAR_CARDS);
            player2Deck.RemoveRange(0, NUM_WAR_CARDS);
            /*foreach (var item in cardsToAdd)
            {
                Console.Write(item + ", ");
            }
            Console.WriteLine("");*/
            CompareCards();
        }

        /// <summary>
        /// Parse the correct suit and value of the card to print to screen.
        /// </summary>
        /// <param name="player">the player whose card this is</param>
        /// <param name="value">the base numerical value of the card</param>
        /// <param name="suit">the numerical suit of the card</param>
        private void PrintCard(string player, int value, int suit)
        {
            string valueStr = (value + CARD_NUM_OFFSET).ToString();
            if (value >= (int)HighCards.JACK)
            {
                switch ((HighCards)value)
                {
                    case HighCards.JACK:
                        valueStr = HighCards.JACK.ToString().ToLower();
                        break;
                    case HighCards.QUEEN:
                        valueStr = HighCards.QUEEN.ToString().ToLower();
                        break;
                    case HighCards.KING:
                        valueStr = HighCards.KING.ToString().ToLower();
                        break;
                    default:
                        valueStr = HighCards.ACE.ToString().ToLower();
                        break;
                }
            }

            string suitStr;
            switch ((Suits)suit)
            {
                case Suits.CLUBS:
                    suitStr = Suits.CLUBS.ToString().ToLower();
                    break;
                case Suits.HEARTS:
                    suitStr = Suits.HEARTS.ToString().ToLower();
                    break;
                case Suits.SPADES:
                    suitStr = Suits.SPADES.ToString().ToLower();
                    break;
                default:
                    suitStr = Suits.DIAMONDS.ToString().ToLower();
                    break;
            }

            //Console.WriteLine("Value: " + value);
            //Console.WriteLine("Suit: " + suit);
            Console.WriteLine(player + " has " + valueStr + " of " + suitStr);
        }

        /// <summary>
        /// Print the results of each round.
        /// </summary>
        /// <param name="playerName">the player that won the round</param>
        private void PrintRoundWinner(string playerName)
        {
            if (!manualPlay)
            {
                return;
            }
            string winStr = " wins this round.";
            if (isWar)
            {
                winStr = " wins the war!";
            }
            Console.WriteLine(Environment.NewLine + playerName + winStr + Environment.NewLine);
        }
    }
}
