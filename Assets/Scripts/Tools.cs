using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Tools {
	public static Quaternion SnapTangents(ref Vector3 Right, ref Vector3 Up, ref Vector3 Forward) {
		// Orthonormalize the normal with the player's direction and return the rotation.
		Vector3.OrthoNormalize(ref Up, ref Forward, ref Right);
		Matrix4x4 rotationMatrix = Matrix4x4.identity;
		rotationMatrix.SetColumn(0,Right);
		rotationMatrix.SetColumn(1,Up);
		rotationMatrix.SetColumn(2,Forward);
		return QuaternionFromMatrix(rotationMatrix);//rotationMatrix.rotation;
	}

	public static Vector3 TransformVector(Quaternion quat, Vector3 vect) {
		// Transform a vector (possibly equivalent to just quat * vector in Unity, should test this)
		Quaternion vectQuat = Quaternion.identity;
		vectQuat.x = vect.x;
		vectQuat.y = vect.y;
		vectQuat.z = vect.z;
		vectQuat.w = 0.0f;
		Quaternion result = quat * vectQuat * Quaternion.Inverse(quat);
		return new Vector3(result.x,result.y,result.z);
	}

	// Create a quaternion from a matrix. Acts more stable than doing "mat.rotation".
	public static Quaternion QuaternionFromMatrix(Matrix4x4 m) {
		return Quaternion.LookRotation(m.GetColumn(2), m.GetColumn(1));
	}
}
