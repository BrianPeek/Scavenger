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
		VisionServiceClient client = new VisionServiceClient("a77fea5d312647b1927c6bc61e069794", "http://southcentralus.api.cognitive.microsoft.com/vision/v1.0");
		AnalysisResult result = await client.DescribeAsync(imgStream);
		return result;
	}
}
