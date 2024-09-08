using UnityEngine;

public class ReadInput : MonoBehaviour
{
    public CohereIntegration cohereIntegration;  
    private string input;

    // Method called when the button is pressed
    public void ReadStringInput()
    {
        input = GameObject.Find("JournalField").GetComponent<UnityEngine.UI.InputField>().text;
        Debug.Log("Journal Input: " + input);

        StartCoroutine(cohereIntegration.SendJournalToCohere(input, ProcessCohereResponse));
    }

    private void ProcessCohereResponse(string response)
    {
        Debug.Log("Summarized response: " + response);

        // Use the summary to generate objects in the 3D world (next step)
        Generate3DWorld(response);
    }

    // Placeholder for the 3D world generation logic
    private void Generate3DWorld(string summary)
    {
        // Convert summary into 3D objects in the game (step 4)
        Debug.Log("Generating 3D world based on the summary...");
    }
}
