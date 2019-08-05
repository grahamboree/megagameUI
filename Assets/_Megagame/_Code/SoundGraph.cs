using System.Collections.Generic;
using Vectrosity;
using UnityEngine;

public class SoundGraph : MonoBehaviour {
    [SerializeField] RectTransform Container;
    [SerializeField] int numSamples = 20;

    [SerializeField] float xResolution = 0.1f;
    [SerializeField] float yOffset = 0f;
    [SerializeField] float scrollSpeed = 0.01f;
    [SerializeField] int octaves = 4;
    [SerializeField] float persistence = 0.1f;

    VectorLine line;

    void Start() {
        line = new VectorLine("graph", new List<Vector3>(), 1.0f) {
            lineType = LineType.Continuous
        };
    }

    void Update() {
        var corners = new Vector3[4];
        Container.GetWorldCorners(corners);

        float deltaX = corners[2].x - corners[0].x;
        float deltaY = corners[1].y - corners[0].y;

        float xIncrement = deltaX / numSamples;

        line.points3.Clear();
        for (int i = 0; i <= numSamples; ++i) {
            line.points3.Add(corners[0] + new Vector3(i * xIncrement, noiseValue(i) * deltaY, 0));
        }

        line.Draw3D();
    }

    float noiseValue(int sampleNum) {
        float x =  + xResolution * ((float) sampleNum / numSamples);
        float y = Time.unscaledTime * scrollSpeed + yOffset;

        float total = 0;
        float frequency = 1;
        float amplitude = 1;
        float maxValue = 0; // Used for normalizing result to 0.0 - 1.0
        for (int i = 0; i < octaves; i++) {
            total += Mathf.PerlinNoise(x * frequency, y * frequency) * amplitude;

            maxValue += amplitude;

            amplitude *= persistence;
            frequency *= 2;
        }

        return total / maxValue;


//        return Mathf.PerlinNoise(Time.unscaledTime * scrollSpeed + xResolution * ((float) i / numSamples), yOffset);
    }

    public static Rect RectTransformToScreenSpace(RectTransform transform) {
        var size = Vector2.Scale(transform.rect.size, transform.lossyScale);
        var rect = new Rect(transform.position.x, Screen.height - transform.position.y, size.x, size.y);
        rect.x -= (transform.pivot.x * size.x);
        rect.y -= ((1.0f - transform.pivot.y) * size.y);
        return rect;
    }
}
