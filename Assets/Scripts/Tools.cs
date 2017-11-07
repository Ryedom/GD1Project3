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

	// Create a quaternion from a matrix. Acts more stable than doing "mat.rotation".
	public static Quaternion QuaternionFromMatrix(Matrix4x4 m) {
		return Quaternion.LookRotation(m.GetColumn(2), m.GetColumn(1));
	}
}
