using UnityEngine;

public class AnimationCurveSampler
{
    private readonly AnimationCurve _densityCurve;
    private readonly IntegrateFunc _integratedDensity;

    private float MinT => _densityCurve.keys[0].time;
    private float MaxT => _densityCurve.keys[_densityCurve.length - 1].time;

    public AnimationCurveSampler(AnimationCurve curve, int integrationSteps = 100)
    {
        _densityCurve = curve;
        _integratedDensity = new IntegrateFunc(curve.Evaluate, curve.keys[0].time, curve.keys[curve.length - 1].time, integrationSteps);
    }

    private float Invert(float x)
    {
        float targetValue = _densityCurve.Evaluate(x);
        float lower = 0f;
        float upper = 1f;
        const float precision = 0.00001f;

        while (upper - lower > precision)
        {
            float mid = (lower + upper) / 2f;
            float midValue = _densityCurve.Evaluate(mid);

            if (midValue > targetValue)
            {
                upper = mid;
            }
            else if (midValue < targetValue)
            {
                lower = mid;
            }
            else
            {
                return mid;
            }
        }

        return (lower + upper) / 2f;
    }

    public float TransformUnit(float unitValue)
    {
        return Invert(unitValue);
    }

    public float RandomSample()
    {
        return Invert(Random.value);
    }
}