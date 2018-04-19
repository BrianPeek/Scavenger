using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Vision.Contract;
using UnityEngine;

public class Cognitive
{
	public static async Task<AnalysisResult> CheckImage(Stream imgStream)
	{
		VisionServiceClient client = new VisionServiceClient("9c41596b4b2d47f8bc2e5b6f0e6baef9", "http://eastus.api.cognitive.microsoft.com/vision/v1.0");
		AnalysisResult result = await client.DescribeAsync(imgStream);
		return result;
	}
}
