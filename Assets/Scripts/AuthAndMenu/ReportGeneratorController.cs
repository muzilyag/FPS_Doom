using UnityEngine;
using System.IO;
using ReportGenerator;
using UnityEngine.UI;
using QuestPDF.Infrastructure;

public class ReportGeneratorController : MonoBehaviour
{
    public Text FeedbackText; 

    public void GenerateReport()
    {
        QuestPDF.Settings.License = LicenseType.Community;
        string jsonPath = Path.Combine(Application.persistentDataPath, "users.json");
        string outputPath = Path.Combine(Directory.GetCurrentDirectory(), "report.pdf");
        string currentAccountId = PlayerPrefs.GetString("CurrentAccountId");

        try
        {
            Report.GenerateReport(jsonPath, outputPath, currentAccountId);
            FeedbackText.text = "����� ������: " + outputPath;
        }
        catch (System.Exception ex)
        {
            string errorMessage = "������: " + ex.Message;
            Debug.LogError(errorMessage);

            // ����� ������ � UI
            if (FeedbackText != null)
                FeedbackText.text = errorMessage;
        }
    }
}