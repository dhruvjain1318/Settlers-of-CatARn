using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceSideDecetor : MonoBehaviour {

    public List<Vector3> directions;
    public List<int> sideValues;

    void Awake()
    {
        // For the sake of this example we assume a regular cube dice if 
        // directions haven't been specified in the editor. Sum of opposite
        // sides is 7, haven't consider exact real layout though.
        if (directions.Count == 0)
        {
            // Object space directions
            directions.Add(Vector3.up);
            sideValues.Add(2); // up
            directions.Add(Vector3.down);
            sideValues.Add(6); // down

            directions.Add(Vector3.left);
            sideValues.Add(2); // left
            directions.Add(Vector3.right);
            sideValues.Add(4); // right

            directions.Add(Vector3.forward);
            sideValues.Add(6); // fw
            directions.Add(Vector3.back);
            sideValues.Add(1); // back
        }

        // Assert equal side of lists
        if (directions.Count != sideValues.Count)
        {
            Debug.LogError("Not consistent list sizes");
        }
    }

    void Start()
    {
        // For sake of example, get number based on current orientation
        // This makes it possible to test by just rotating it in the editor and hitting play
        // Allowing 30 degrees error so will give (the side that is mostly upwards)
        // but will give -1 on "tie"
        Debug.Log("The side world up has value: " + GetNumber(Vector3.up, 30f));
    }

    private void Update()
    {
        Debug.Log("The side world up has value: " + GetNumber(Vector3.up, 30f));

    }

    // Gets the number of the side pointing in the same direction as the reference vector,
    // allowing epsilon degrees error.
    public int GetNumber(Vector3 referenceVectorUp, float epsilonDeg = 5f)
    {
        // here I would assert lookup is not empty, epsilon is positive and larger than smallest possible float etc
        // Transform reference up to object space
        Vector3 referenceObjectSpace = transform.InverseTransformDirection(referenceVectorUp);

        // Find smallest difference to object space direction
        float min = float.MaxValue;
        int mostSimilarDirectionIndex = -1;
        for (int i = 0; i < directions.Count; ++i)
        {
            float a = Vector3.Angle(referenceObjectSpace, directions[i]);
            if (a <= epsilonDeg && a < min)
            {
                min = a;
                mostSimilarDirectionIndex = i;
            }
        }

        // -1 as error code for not within bounds
        return (mostSimilarDirectionIndex >= 0) ? sideValues[mostSimilarDirectionIndex] : -1;
    }
}
