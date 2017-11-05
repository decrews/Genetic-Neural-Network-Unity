using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AgentManager : MonoBehaviour {

	public GameObject botPrefab;
	public GameObject genText;

	private bool isTraning = false;
	public int populationSize = 10;
	private int generationNumber = 0;
	private int[] layers;
	private List<NeuralNetwork> nets;
	public List<TrainController> botsList = null;
	public float bestFit = 0;

	void Start() {
		// Set the input layer of the neural network size equal to the amount
		// of platforms and enemies.
		GameObject[] grounds = GameObject.FindGameObjectsWithTag ("Ground");
		GameObject[] enemies = GameObject.FindGameObjectsWithTag ("Enemy");

		// Input should be double the number of platforms and enemies (x, y) for each
		int input = (grounds.Length + enemies.Length) * 2;

		Debug.Log (input);
		layers = new int[] { input, 10, 10, 3 }; // enough inputs for all platforms and enemies, 3 movement outputs
	}

	void Update ()
	{
		if (botsList.Count == 0)
		{
			if (generationNumber == 0)
			{
				InitNeuralNetwork();
			}
			else
			{
				nets.Sort();

				// Check if fitness is new record
				if (nets [nets.Count - 1].GetFitness () > bestFit) {
					bestFit = nets [nets.Count - 1].GetFitness ();
					GameObject.Find ("Fit").GetComponent<Text> ().text = bestFit.ToString();
				}

				for (int i = 0; i < populationSize / 2; i++)
				{
					nets[i] = new NeuralNetwork(nets[i+(populationSize / 2)]);
					nets[i].Mutate();

					//too lazy to write a reset neuron matrix values method....so just going to make a deepcopy lol
					nets[i + (populationSize / 2)] = new NeuralNetwork(nets[i + (populationSize / 2)]);
				}

				for (int i = 0; i < populationSize; i++)
				{
					nets[i].SetFitness(0f);
				}
			}
				
			generationNumber++;

			Debug.Log ("Generation number: " + generationNumber);
			GameObject.Find ("Gen").GetComponent<Text> ().text = generationNumber.ToString();

			CreateBots();
		}
	}


	private void CreateBots()
	{
		if (botsList != null)
		{
			if (botsList.Count != 0) {
				for (int i = 0; i < botsList.Count; i++)
				{
					GameObject.Destroy(botsList[i].gameObject);
				}
			}
		}

		botsList = new List<TrainController>();

		for (int i = 0; i < populationSize; i++)
		{
			TrainController bot = ((GameObject)Instantiate(botPrefab, new Vector3(2, 1, 0), Quaternion.identity)).GetComponent<TrainController>();
			bot.Init(nets[i]);
			botsList.Add(bot);
		}

	}

	void InitNeuralNetwork()
	{
		//population must be even, just setting it to 20 incase it's not
		if (populationSize % 2 != 0)
		{
			populationSize = 20; 
		}

		nets = new List<NeuralNetwork>();


		for (int i = 0; i < populationSize; i++)
		{
			NeuralNetwork net = new NeuralNetwork(layers);
			net.Mutate();
			nets.Add(net);
		}
	}

}
